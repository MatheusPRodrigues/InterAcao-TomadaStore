using MongoDB.Bson;
using MongoDB.Driver;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.DTOs.SaleRequestDTO;
using TomadaStore.Models.Models;
using TomadaStore.SaleAPI.Data;
using TomadaStore.SaleAPI.Repository.Interfaces;

namespace TomadaStore.SaleAPI.Repository
{
    public class SaleRepository : ISaleRepository
    {
        private readonly ILogger<SaleRepository> _logger;
        private readonly IMongoCollection<Sale> _saleCollection;

        public SaleRepository(
            ILogger<SaleRepository> logger,
            ConnectionDB connectionDB
        )
        {
            _logger = logger;
            _saleCollection = connectionDB.GetSaleCollection();
        }

        public async Task CreateSaleAsync(CustomerResponseDTO customerDTO, ProductResponseDTO productDTO, SaleRequestDTO saleDTO)
        {
            try
            {
                var customer = new Customer(
                    customerDTO.Id,
                    customerDTO.FirstName,
                    customerDTO.LastName,
                    customerDTO.Email,
                    customerDTO.PhoneNumber,
                    customerDTO.Status
                );

                var products = new List<Product>();
                var product = new Product(
                    productDTO.Id,
                    productDTO.Name,
                    productDTO.Description,
                    productDTO.Price,
                    new Category(
                        productDTO.Category.Id,
                        productDTO.Category.Name,
                        productDTO.Category.Description
                    )
                );
                products.Add(product);

                await _saleCollection.InsertOneAsync(new Sale(
                    customer,
                    products,
                    productDTO.Price
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating sale: {ex.Message}");
                throw;
            }
        }
    }
}
