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
        private readonly IHttpClientFactory _httpClientFactory;

        public SaleService(
            ILogger<SaleService> logger,
            ISaleRepository saleRepository,
            IHttpClientFactory httpClientFactory
        )
        {
            _logger = logger;
            _saleRepository = saleRepository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task CreateSaleAsync(int idCustomer, SaleRequestDTO saleDTO)
        {
            try
            {
                var responseCustomer = await _httpClientFactory
                    .CreateClient("CustomerAPI")
                    .GetFromJsonAsync<CustomerResponseDTO>(idCustomer.ToString());

                var listProducts = new List<ProductResponseDTO>();

                foreach (var p in saleDTO.ProductsId)
                {
                    var responseProduct = await _httpClientFactory
                        .CreateClient("ProductAPI")
                        .GetFromJsonAsync<ProductResponseDTO>(p);
                    listProducts.Add(responseProduct);
                }

                await _saleRepository.CreateSaleAsync(responseCustomer, listProducts, saleDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating a sale: {ex.Message}");
                throw;
            }
        }
    }
}
