using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TomadaStore.SaleConsumerAPI.Services.Interface;

namespace TomadaStore.SaleConsumerAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SaleConsumerController : ControllerBase
    {
        private readonly ILogger<SaleConsumerController> _logger;
        private readonly ISaleConsumerService _saleService;

        public SaleConsumerController(
            ILogger<SaleConsumerController> logger,
            ISaleConsumerService saleService
        )
        {
            _logger = logger;
            _saleService = saleService;
        }

        [HttpGet]
        public async Task<ActionResult> SaveSalesInBD()
        {
            try
            {
                await _saleService.ConsumeApprovalsQueueAsync();
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Don't have messages in queue for persist: {ex.Message}");
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurring while consume payment queue: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
