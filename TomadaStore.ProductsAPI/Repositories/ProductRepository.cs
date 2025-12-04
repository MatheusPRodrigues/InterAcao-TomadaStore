using MongoDB.Bson;
using MongoDB.Driver;
using TomadaStore.Models.DTOs.Category;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.Models;
using TomadaStore.ProductAPI.Data;
using TomadaStore.ProductAPI.Repositories.Interfaces;

namespace TomadaStore.ProductAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ILogger<ProductRepository> _logger;
        private readonly IMongoCollection<Product> _productCollection;

        public ProductRepository(
            ILogger<ProductRepository> logger,
            ConnectionDB connectionDB)
        {
            _logger = logger;
            _productCollection = connectionDB.GetProductCollection();
        }

        public async Task CreateProductAsync(ProductRequestDTO productDto)
        {
            try
            {
                await _productCollection.InsertOneAsync(new Product(
                    productDto.Name,
                    productDto.Description,
                    productDto.Price,
                    new Category (
                        productDto.Category.Name,    
                        productDto.Category.Description    
                    )
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating product: {ex.Message}");
                throw;
            }
        }

        public Task<List<ProductResponseDTO>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ProductResponseDTO> GetProductByIdAsync(ObjectId id)
        {
            try
            {
                var result = await _productCollection.FindAsync<Product>(p => p.Id == id).Result.FirstOrDefaultAsync();
                return new ProductResponseDTO
                {
                    Id = result.Id.ToString(),
                    Name = result.Name,
                    Description = result.Description,
                    Price = result.Price,
                    Category = new CategoryResponseDTO
                    {
                        Id = result.Category.Id.ToString(),
                        Name = result.Category.Name,
                        Description = result.Description,
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get one product: {ex.Message}");
                throw;
            }
        }
    }
}
