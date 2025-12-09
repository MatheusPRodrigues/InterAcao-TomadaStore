using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TomadaStore.PaymentAPI.Services.Interfaces;

namespace TomadaStore.PaymentAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentService _paymentService;

        public PaymentController(
            ILogger<PaymentController> logger,
            IPaymentService paymentService
        )
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<ActionResult> ProcessOrderSalesAsync()
        {
            try
            {
                await _paymentService.ProcessOrderSalesQueueAsync();
                return Ok(new { Message = "Fila de compras processada com sucesso!" });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning($"Warning: {ex.Message}");
                return StatusCode(204, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurring while process queue of order sales: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
