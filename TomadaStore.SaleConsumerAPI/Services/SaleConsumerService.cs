using MongoDB.Bson.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.Models.Models;
using TomadaStore.SaleConsumerAPI.Repository.Interface;
using TomadaStore.SaleConsumerAPI.Services.Interface;
using TomadaStore.Utils.Helpers;

namespace TomadaStore.SaleConsumerAPI.Services
{
    public class SaleConsumerService : ISaleConsumerService
    {
        private readonly ILogger<SaleConsumerService> _logger;
        private readonly ISaleRepository _saleRepository;
        private readonly SaleConverter _saleConverter;

        public SaleConsumerService(
            ILogger<SaleConsumerService> logger,
            ISaleRepository saleRepository,
            SaleConverter saleConverter
        )
        {
            _logger = logger;
            _saleRepository = saleRepository;
            _saleConverter = saleConverter;
        }

        public async Task ConsumeApprovalsQueueAsync()
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

                var result = await channel.QueueDeclarePassiveAsync("approvalsQueue");
                if (result.MessageCount == 0)
                    throw new InvalidOperationException("Não há compras na fila de aprovados para serem persistidos no BD");

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var content = BsonSerializer.Deserialize<SaleResponseConsumerDTO>(message);
                    if (content is not null)
                    {
                        var sale = _saleConverter.ConvertForSale(content);
                        await _saleRepository.PersisitSaleAsync(sale);
                    }
                };

                await channel.BasicConsumeAsync(
                    queue: "approvalsQueue",
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
    }
}
