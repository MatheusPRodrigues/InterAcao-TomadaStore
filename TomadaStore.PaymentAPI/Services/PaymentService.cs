using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.Models.Models;
using TomadaStore.PaymentAPI.Services.Interfaces;
using TomadaStore.Utils.Helpers;

namespace TomadaStore.PaymentAPI.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ILogger<PaymentService> _logger;
        private readonly SaleConverter _saleConverter;

        public PaymentService(
            ILogger<PaymentService> logger,
            SaleConverter saleConverter)
        {
            _logger = logger;
            _saleConverter = saleConverter;
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
                        var sale = _saleConverter.ConvertForSale(content);
                        if (sale.TotalPrice < 1000)
                        {
                            sale.SetIsApproved(true);
                            await AddingQueueApprovedSales(sale);
                        }
                        else
                        {
                            sale.SetIsApproved(false);
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
    }
}
