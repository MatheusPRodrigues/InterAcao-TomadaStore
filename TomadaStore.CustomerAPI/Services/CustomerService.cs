using TomadaStore.CustomerAPI.Repository;
using TomadaStore.CustomerAPI.Repository.Interfaces;
using TomadaStore.CustomerAPI.Services.Interfaces;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.Models;

namespace TomadaStore.CustomerAPI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(
            ILogger<CustomerService> logger,
            ICustomerRepository customerRepository
        )
        {
            _logger = logger;
            _customerRepository = customerRepository;
        }

        public async Task DesactiveCustomerAsync(int id)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(id);
                if (customer is null)
                    throw new ArgumentException("Cliente não encontrado!");

                await _customerRepository.DesactiveCustomerAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<List<CustomerResponseDTO>> GetAllCustomerAsync()
        {
            try
            {
                var customers = await _customerRepository.GetAllCustomerAsync();
                return customers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<CustomerResponseDTO> GetCustomerByIdAsync(int id)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(id);
                
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<int> InsertCustomerAsync(CustomerRequestDTO customer)
        {
            try
            {
                return await _customerRepository.InsertCustomerAsync(customer);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
