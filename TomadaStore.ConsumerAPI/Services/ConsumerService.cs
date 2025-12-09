using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TomadaStore.ConsumerAPI.Repository.Intefaces;
using TomadaStore.ConsumerAPI.Services.Intefaces;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.Models.DTOs.SaleRequestDTO;
using TomadaStore.Models.Models;

namespace TomadaStore.ConsumerAPI.Services
{
    public class ConsumerService : IConsumerService
    {
        private readonly ILogger<ConsumerService> _logger;
        private readonly ISaleRepository _saleRepository;

        public ConsumerService
            (ILogger<ConsumerService> logger,
            ISaleRepository saleRepository)
        {
            _logger = logger;
            _saleRepository = saleRepository;
        }

        public async Task GetSaleInQueueAsync()
        {
            try
            {
                var salesList = new Queue<SaleResponseConsumerDTO>();

                var factory = new ConnectionFactory { HostName = "localhost" };
                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(
                    queue: "sales",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var result = await channel.QueueDeclarePassiveAsync("sales");
                if (result.MessageCount == 0)
                    throw new InvalidOperationException("Não há dados para serem consumidos");

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(message);
                    
                    var content = BsonSerializer.Deserialize<SaleResponseConsumerDTO>(message);
                    if (content is not null)
                    {
                        var sale = ConvertForSale(content);
                        _saleRepository.SaveSaleInCollectionAsync(sale);
                    }
                        

                    return Task.CompletedTask;
                };

                await channel.BasicConsumeAsync(
                    queue: "sales",
                    autoAck: true,
                    consumer: consumer
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurring while save Sale {ex.Message}");
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
