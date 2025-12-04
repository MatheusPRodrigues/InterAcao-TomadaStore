using MongoDB.Bson;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.DTOs.SaleRequestDTO;

namespace TomadaStore.SaleAPI.Repository.Interfaces
{
    public interface ISaleRepository
    {
        Task CreateSaleAsync(
            CustomerResponseDTO customerDTO,
            ProductResponseDTO productDTO,
            SaleRequestDTO saleDTO
        );
    }
}
