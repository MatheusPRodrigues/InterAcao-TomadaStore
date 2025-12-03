using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.Models;

namespace TomadaStore.CustomerAPI.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<int> InsertCustomerAsync(CustomerRequestDTO customer);
        Task<List<CustomerResponseDTO>> GetAllCustomerAsync();
        Task<CustomerResponseDTO> GetCustomerByIdAsync(int id);
        Task DesactiveCustomerAsync(int id);

    }
}