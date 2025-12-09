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

        public async Task CreateSaleAsync(CustomerResponseDTO customerDTO, List<ProductResponseDTO> productsDTO, SaleRequestDTO saleDTO)
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

                foreach (var p in productsDTO)
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

                await _saleCollection.InsertOneAsync(new Sale(
                    customer,
                    products
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating sale: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Sale>> GetAllSalesAsync()
        {
            try
            {
                return await _saleCollection.FindAsync(s => true).Result.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while get all sales: {ex.Message}");
                throw;
            }
        }

        public async Task SaveSaleInBdAsync(Sale sale)
        {
            try
            {
                await _saleCollection.InsertOneAsync(sale);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating sale: {ex.Message}");
                throw;
            }
        }
    }
}
