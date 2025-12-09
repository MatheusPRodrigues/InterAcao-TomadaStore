using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TomadaStore.PaymentAPI.Services.Interfaces;

namespace TomadaStore.PaymentAPI.Controllers
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
                return Ok(new { Message = "Fila de compras processada com sucesso!" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurring while process queue of order sales: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
