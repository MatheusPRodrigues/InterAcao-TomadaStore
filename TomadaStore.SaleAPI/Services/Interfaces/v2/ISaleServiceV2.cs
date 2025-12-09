using Microsoft.AspNetCore.Mvc;
using TomadaStore.Models.DTOs.SaleRequestDTO;
using TomadaStore.Models.Models;

namespace TomadaStore.SaleAPI.Services.Interfaces.v2
{
    public interface ISaleServiceV2
    {
        public Task CreateSaleAsync(int idCustomer, SaleRequestDTO saleDTO);
    }
}
