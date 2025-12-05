using MongoDB.Bson;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.DTOs.SaleRequestDTO;
using TomadaStore.Models.Models;

namespace TomadaStore.SaleAPI.Repository.Interfaces
{
    public interface ISaleRepository
    {
        Task CreateSaleAsync(
            CustomerResponseDTO customerDTO,
            List<ProductResponseDTO> productsDTO,
            SaleRequestDTO saleDTO
        );
        Task<List<Sale>> GetAllSalesAsync();
    }
}
