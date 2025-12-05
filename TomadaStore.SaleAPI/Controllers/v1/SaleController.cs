using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TomadaStore.Models.DTOs.SaleRequestDTO;
using TomadaStore.SaleAPI.Services.Interfaces;

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
            catch (Exception ex)
            {
                _logger.LogError($"Error occurring while create a sale: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
