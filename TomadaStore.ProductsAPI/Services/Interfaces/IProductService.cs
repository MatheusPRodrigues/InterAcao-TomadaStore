using MongoDB.Bson;
using TomadaStore.Models.DTOs.Product;

namespace TomadaStore.ProductAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductResponseDTO>> GetAllProductsAsync();
        Task<ProductResponseDTO> GetProductByIdAsync(ObjectId id);
        Task CreateProductAsync(ProductRequestDTO productDto);
        Task UpdateProductAsync(string id, ProductRequestDTO productDto);
        Task DeleteProductAsync(string id);
    }
}
