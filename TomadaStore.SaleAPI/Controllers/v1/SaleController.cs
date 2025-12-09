using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TomadaStore.Models.DTOs.SaleRequestDTO;
using TomadaStore.SaleAPI.Services.Interfaces.v1;

namespace TomadaStore.SaleAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ILogger<SaleController> _logger;
        private readonly ISaleService _saleService;

        public SaleController(
            ILogger<SaleController> logger,
            ISaleService saleService
        )
        {
            _logger = logger;
            _saleService = saleService;
        }

        [HttpPost("Customer/{idCustomer}/Products")]
        public async Task<ActionResult> CreateSaleAsync(int idCustomer, [FromBody] SaleRequestDTO saleDTO)
        {
            try
            {
                _logger.LogInformation("Creating a new sale");
                await _saleService.CreateSaleAsync(idCustomer, saleDTO);
                return Created();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning($"validation operations return: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurring while create a sale: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<SaleResponseDTO?>>> GetAllSalesAsync()
        {
            try
            {
                var response = await _saleService.GetAllSalesAsync();
                if (response is null)
                    return NotFound("Nenhuma venda encontrada!");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurring while create a sale: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
