using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.Models;

namespace TomadaStore.CustomerAPI.Repository.Interfaces
{
    public interface ICustomerRepository
    {
        Task<int> InsertCustomerAsync(CustomerRequestDTO customer);
        Task<List<CustomerResponseDTO>> GetAllCustomerAsync();
        Task<CustomerResponseDTO?> GetCustomerByIdAsync(int id);
        Task DesactiveCustomerAsync(int id);
    }
}