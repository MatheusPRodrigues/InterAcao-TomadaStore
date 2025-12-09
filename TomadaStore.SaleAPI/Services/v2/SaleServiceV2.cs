using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.DTOs.SaleRequestDTO;
using TomadaStore.Models.Models;
using TomadaStore.SaleAPI.Services.Interfaces.v2;
using TomadaStore.SaleAPI.Repository.Interfaces;


namespace TomadaStore.SaleAPI.Services.v2
{
    public class SaleServiceV2 : ISaleServiceV2
    {
        private readonly ILogger<SaleServiceV2> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISaleRepository _saleRepository;
        
        public SaleServiceV2(
            ILogger<SaleServiceV2> logger,
            IHttpClientFactory httpClientFactory,
            ISaleRepository saleRepository
        )
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _saleRepository = saleRepository;
        }

        public async Task CreateSaleAsync(int idCustomer, SaleRequestDTO saleDTO)
        {
            try
            {
                var responseCustomer = await _httpClientFactory
                   .CreateClient("CustomerAPI")
                   .GetFromJsonAsync<CustomerResponseDTO>(idCustomer.ToString());
                if (responseCustomer is null)
                    throw new InvalidOperationException("O Cliente informado não foi encontrado!");

                var listProducts = new List<ProductResponseDTO>();
                foreach (var p in saleDTO.ProductsId)
                {
                    var responseProduct = await _httpClientFactory
                        .CreateClient("ProductAPI")
                        .GetFromJsonAsync<ProductResponseDTO>(p);
                    listProducts.Add(responseProduct);
                }
                if (listProducts.All(p => p is null))
                    throw new InvalidOperationException("Não foram informados produtos não encontrados!");

                var customer = new Customer(
                    responseCustomer.Id,
                    responseCustomer.FirstName,
                    responseCustomer.LastName,
                    responseCustomer.Email,
                    responseCustomer.PhoneNumber,
                    responseCustomer.Status
                );

                var products = new List<Product>();

                foreach (var p in listProducts)
                {
                    var product = new Product(
                        p.Id,
                        p.Name,
                        p.Description,
                        p.Price,
                        new Category(
                            p.Category.Id,
                            p.Category.Name,
                            p.Category.Description
                        )
                    );
                    products.Add(product);
                }

                var sale = new Sale(
                    customer,
                    products
                );

                await ProduceSaleAsync(sale);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating a sale: {ex.Message}");
                throw;
            }
        }

        public async Task PersistApprovedSales()
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
                    throw new InvalidOperationException("Não há compras para serem persistidas no BD");

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var content = BsonSerializer.Deserialize<SaleResponseConsumerDTO>(message);
                    if (content is not null)
                    {
                        var sale = ConvertForSale(content);
                        await _saleRepository.SaveSaleInBdAsync(sale);
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

        public async Task ProduceSaleAsync(Sale sale)
        {
            try
            {
                var factory = new ConnectionFactory { HostName = "localhost"};
                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(
                    queue: "orderSales",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var saleSerialize = sale.ToJson();
                var body = Encoding.UTF8.GetBytes(saleSerialize);

                await channel.BasicPublishAsync(
                    exchange: string.Empty, //seria um cluster de filas
                    routingKey: "orderSales",
                    body: body
                );
                Console.WriteLine("Sale enviado para fila do Rabbit com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating a sale: {ex.Message}");
                throw;
            }
        }
    }
}
