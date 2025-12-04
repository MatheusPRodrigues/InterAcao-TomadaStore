using MongoDB.Bson;
using System.Text.Json;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.DTOs.SaleRequestDTO;
using TomadaStore.Models.Models;
using TomadaStore.SaleAPI.Repository.Interfaces;
using TomadaStore.SaleAPI.Services.Interfaces;

namespace TomadaStore.SaleAPI.Services
{
    public class SaleService : ISaleService
    {
        private readonly ILogger<SaleService> _logger;
        private readonly ISaleRepository _saleRepository;
        private readonly HttpClient _httpClientProduct;
        private readonly HttpClient _httpClientCustomer;

        public SaleService(
            ILogger<SaleService> logger,
            ISaleRepository saleRepository,
            HttpClient httpClientProduct,
            HttpClient httpClientCustomer
        )
        {
            _logger = logger;
            _saleRepository = saleRepository;
            _httpClientProduct = httpClientProduct;
            _httpClientCustomer = httpClientCustomer;
        }

        public async Task CreateSaleAsync(int idCustomer, string idProduct, SaleRequestDTO saleDTO)
        {
            try
            {
                var responseCustomer = await _httpClientCustomer
                    .GetFromJsonAsync<CustomerResponseDTO>(idCustomer.ToString());

                var responseProduct = await _httpClientProduct
                    .GetFromJsonAsync<ProductResponseDTO>(idCustomer.ToString());

                await _saleRepository.CreateSaleAsync(responseCustomer, responseProduct, saleDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating a sale: {ex.Message}");
                throw;
            }
        }
    }
}
