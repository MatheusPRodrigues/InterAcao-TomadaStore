using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TomadaStore.CustomerAPI.Services;
using TomadaStore.CustomerAPI.Services.Interfaces;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.Models;

namespace TomadaStore.CustomerAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(
            ILogger<CustomerController> logger,
            ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateCustomerAsync([FromBody] CustomerRequestDTO customer)
        {
            try
            {
                _logger.LogInformation("Creating a new customer");
                var id = await _customerService.InsertCustomerAsync(customer);
                var createdCustomer = await _customerService.GetCustomerByIdAsync(id);
                Console.WriteLine(nameof(GetCustomerByIdAsync));
                return Created();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new customer" + ex.Message);
                return Problem(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<CustomerResponseDTO>>> GetAllCustomerAsync()
        {
            try
            {
                var customers = await _customerService.GetAllCustomerAsync();
                if (customers is null || customers.Count == 0)
                    return NotFound("Não há clientes cadastrados!"); 

                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all customer " + ex.Message);
                return Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponseDTO>> GetCustomerByIdAsync(int id)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(id);
                if (customer is null)
                    return NotFound("Cliente não encontrado");

                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving a customer by id " + ex.Message);
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DesactiveCustomerAsync(int id)
        {
            try
            {
                await _customerService.DesactiveCustomerAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when disabling client: " + ex.Message);
                return Problem(ex.Message);
            }
        }
    }
}
