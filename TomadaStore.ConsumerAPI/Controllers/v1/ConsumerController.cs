using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TomadaStore.ConsumerAPI.Services.Intefaces;

namespace TomadaStore.ConsumerAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ConsumerController : ControllerBase
    {
        private readonly ILogger<ConsumerController> _logger;
        private readonly IConsumerService _consumerService;

        public ConsumerController(
            ILogger<ConsumerController> logger,
            IConsumerService consumerService)
        {
            _logger = logger;
            _consumerService = consumerService;
        }

        [HttpGet("Sale")]
        public async Task<ActionResult> SaveSaleAsync()
        {
            try
            {
                await _consumerService.GetSaleInQueueAsync();
                return Ok(new { Message = "Compras foram consumidas com sucesso!" });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning($"Warning: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurring while save Sale {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
