using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TomadaStore.Models.DTOs.SaleRequestDTO;
using TomadaStore.Models.Models;

namespace TomadaStore.SaleAPI.Services.Interfaces.v1
{
    public interface ISaleService
    {
        Task CreateSaleAsync(int idCustomer, SaleRequestDTO saleDTO);
        Task<List<SaleResponseDTO?>> GetAllSalesAsync();
    }
}
