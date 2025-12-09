using MongoDB.Bson;
using System.Text.Json;
using TomadaStore.Models.DTOs.Category;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.DTOs.SaleRequestDTO;
using TomadaStore.Models.Models;
using TomadaStore.SaleAPI.Repository.Interfaces;
using TomadaStore.SaleAPI.Services.Interfaces.v1;

namespace TomadaStore.SaleAPI.Services.v1
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
                if (responseCustomer is null)
                    throw new InvalidOperationException("O Cliente informado não foi encontrado!");

                var listProducts = new List<ProductResponseDTO>();
                foreach (var p in saleDTO.ProductsId)
                {
                    var responseProduct = await _httpClientFactory
                        .CreateClient("ProductAPI")
                        .GetFromJsonAsync<ProductResponseDTO>(p);
                    listProducts.Add(responseProduct);
                }
                if (listProducts.All(p => p is null))
                    throw new InvalidOperationException("Não foram informados produtos não encontrados!");

                await _saleRepository.CreateSaleAsync(responseCustomer, listProducts, saleDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating a sale: {ex.Message}");
                throw;
            }
        }

        public async Task<List<SaleResponseDTO?>> GetAllSalesAsync()
        {
            try
            {
                var sales = await _saleRepository .GetAllSalesAsync();
                if (sales is null || sales.Count == 0)
                    return null;

                var salesResponse = new List<SaleResponseDTO>();
                foreach (var s in sales)
                {
                    var listProductsInSale = new List<ProductResponseDTO>();
                    foreach (var p in s.Products)
                    {
                        var productDTO = new ProductResponseDTO
                        {
                            Id = p.Id.ToString(),
                            Name = p.Name,
                            Description = p.Description,
                            Price = p.Price,
                            Category = new CategoryResponseDTO
                            {
                                Id = p.Category.Id.ToString(),
                                Name = p.Category.Name,
                                Description = p.Category.Description,
                            }
                        };
                        listProductsInSale.Add(productDTO);
                    }

                    var saleResponseDTO = new SaleResponseDTO
                    {
                        Id = s.Id.ToString(),
                        Customer = new CustomerResponseDTO
                        {
                            Id = s.Customer.Id,
                            FirstName = s.Customer.FirstName,
                            LastName = s.Customer.LastName,
                            Email = s.Customer.Email,
                            PhoneNumber = s.Customer.PhoneNumber,
                            Status = s.Customer.Status
                        },
                        Products = listProductsInSale,
                        SaleDate = TimeZoneInfo.ConvertTimeFromUtc(s.SaleDate, TimeZoneInfo.Local),
                        TotalPrice = s.TotalPrice,
                    };
                    salesResponse.Add(saleResponseDTO);
                }
                return salesResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting all sale: {ex.Message}");
                throw;
            }
        }
    }
}
