using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.Models.Models;
using TomadaStore.PaymentAPI.Services.Interfaces;

namespace TomadaStore.PaymentAPI.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(ILogger<PaymentService> logger)
        {
            _logger = logger;
        }

        public async Task ProcessOrderSalesQueueAsync()
        {
            try
            {
                var factory = new ConnectionFactory { HostName = "localhost" };
                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(
                    queue: "orderSales",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var result = await channel.QueueDeclarePassiveAsync("orderSales");
                if (result.MessageCount == 0)
                    throw new InvalidOperationException("Não há compras para serem processadas");

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                   
                    var content = BsonSerializer.Deserialize<SaleResponseConsumerDTO>(message);
                    if (content is not null)
                    {
                        var sale = ConvertForSale(content);
                        if (sale.TotalPrice < 1000)
                        {
                            Console.WriteLine("Compra aprovada!");
                            await AddingQueueApprovedSales(sale);
                        }
                        else
                        {
                            Console.WriteLine("Compra recusada!");
                            await AddingQueueApprovedSales(sale);
                        }
                    }
                };

                await channel.BasicConsumeAsync(
                    queue: "orderSales",
                    autoAck: true,
                    consumer: consumer
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }
        }

        public async Task AddingQueueApprovedSales(Sale sale)
        {
            try
            {
                var factory = new ConnectionFactory { HostName = "localhost" };
                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(
                    queue: "approvalsQueue",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var saleSerialize = sale.ToJson();
                var body = Encoding.UTF8.GetBytes(saleSerialize);

                await channel.BasicPublishAsync(
                    exchange: string.Empty, //seria um cluster de filas
                    routingKey: "approvalsQueue",
                    body: body
                );
                Console.WriteLine("Sale salva na fila de Sales aprovadas!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating a sale: {ex.Message}");
                throw;
            }
        }

        private Sale ConvertForSale(SaleResponseConsumerDTO dto)
        {

            var customer = new Customer(
                dto.Customer.Id,
                dto.Customer.FirstName,
                dto.Customer.LastName,
                dto.Customer.Email,
                dto.Customer.PhoneNumber,
                dto.Customer.Status
            );

            var products = new List<Product>();

            foreach (var p in dto.Products)
            {
                var product = new Product(
                    p.Id.ToString(),
                    p.Name,
                    p.Description,
                    p.Price,
                    new Category(
                        p.Category.Id.ToString(),
                        p.Category.Name,
                        p.Category.Description
                    )
                );
                products.Add(product);
            }

            var sale = new Sale(
                dto.Id,
                customer,
                products,
                dto.SaleDate,
                dto.TotalPrice
            );

            return sale;
        }
    }
}
