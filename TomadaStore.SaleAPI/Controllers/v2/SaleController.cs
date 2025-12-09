using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TomadaStore.Models.DTOs.SaleRequestDTO;
using TomadaStore.SaleAPI.Services.Interfaces.v2;

namespace TomadaStore.SaleAPI.Controllers.v2
{
    [Route("api/v2/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ILogger<SaleController> _logger;
        private readonly ISaleServiceV2 _saleServiceV2;

        public SaleController(ILogger<SaleController> logger, ISaleServiceV2 saleServiceV2)
        {
            _logger = logger;
            _saleServiceV2 = saleServiceV2;
        }

        [HttpPost("Customer/{idCustomer}/Products")]
        public async Task<ActionResult> CreateSaleAsync(int idCustomer, SaleRequestDTO saleDTO)
        {
            try
            {
                await _saleServiceV2.CreateSaleAsync(idCustomer, saleDTO);
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

        [HttpGet("ApprovedSales")]
        public async Task<ActionResult> PersistApprovedSales()
        {
            try
            {
                await _saleServiceV2.PersistApprovedSales();
                return Ok();
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
    }
}
