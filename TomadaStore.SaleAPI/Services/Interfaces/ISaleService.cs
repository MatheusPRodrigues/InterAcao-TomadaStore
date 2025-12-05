using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TomadaStore.Models.DTOs.SaleRequestDTO;

namespace TomadaStore.SaleAPI.Services.Interfaces
{
    public interface ISaleService
    {
        Task CreateSaleAsync(int idCustomer, SaleRequestDTO saleDTO);
    }
}
