using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TomadaStore.SaleAPI.Controllers.v2
{
    [Route("api/v2/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ILogger<SaleController> _logger;

    }
}
