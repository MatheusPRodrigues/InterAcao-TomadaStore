using MongoDB.Bson;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.DTOs.SaleRequestDTO;
using TomadaStore.Models.Models;
using TomadaStore.SaleAPI.Services.Interfaces.v2;


namespace TomadaStore.SaleAPI.Services.v2
{
    public class SaleServiceV2 : ISaleServiceV2
    {
        private readonly ILogger<SaleServiceV2> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        
        public SaleServiceV2(
            ILogger<SaleServiceV2> logger,
            IHttpClientFactory httpClientFactory
        )
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
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

        public async Task ProduceSaleAsync(Sale sale)
        {
            try
            {
                var factory = new ConnectionFactory { HostName = "localhost"};
                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(
                    queue: "sales",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var saleSerialize = sale.ToJson();
                var body = Encoding.UTF8.GetBytes(saleSerialize);

                await channel.BasicPublishAsync(
                    exchange: string.Empty, //seria um cluster de filas
                    routingKey: "sales",
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
