using Dapper;
using Microsoft.Data.SqlClient;
using TomadaStore.CustomerAPI.Data;
using TomadaStore.CustomerAPI.Repository.Interfaces;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.Models;

namespace TomadaStore.CustomerAPI.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ILogger<CustomerRepository> _logger;
        private readonly SqlConnection _connection;

        public CustomerRepository(
            ILogger<CustomerRepository> logger,
            ConnectionDB connectionDB
        )
        {
            _logger = logger;
            _connection = connectionDB.GetConnection();
        }

        public async Task DesactiveCustomerAsync(int id)
        {
            try
            {
                var sql = "UPDATE Customers SET Status = 0 WHERE Id = @Id";
                await _connection.ExecuteAsync(sql, new {Id = id});
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError($"SQL error when disabling client: {sqlEx.Message}");
                throw new Exception(sqlEx.StackTrace);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when disabling client: {ex.Message}");
                throw new Exception(ex.StackTrace);
            }
        }

        public async Task<List<CustomerResponseDTO>> GetAllCustomerAsync()
        {
            try
            {
                var sql = "SELECT Id, FirstName, LastName, Email, PhoneNumber, [Status] FROM Customers";
                var customers = (await _connection.QueryAsync<CustomerResponseDTO>(sql)).ToList();

                return customers;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError($"SQL Error retrieving customers: {sqlEx.Message}");
                throw new Exception(sqlEx.StackTrace);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving customers: {ex.Message}");
                throw new Exception(ex.StackTrace);
            }
        }

        public async Task<CustomerResponseDTO?> GetCustomerByIdAsync(int id)
        {
            try
            {
                var sql = "SELECT Id, FirstName, LastName, Email, PhoneNumber, [Status] FROM Customers WHERE Id = @Id";
                return await _connection.QueryFirstOrDefaultAsync<CustomerResponseDTO>(sql, new {Id = id});
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError($"SQL Error retrieving customer: {sqlEx.Message}");
                throw new Exception(sqlEx.StackTrace);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving customer: {ex.Message}");
                throw new Exception(ex.StackTrace);
            }
        }

        public async Task<int> InsertCustomerAsync(CustomerRequestDTO customer)
        {
            try
            {
                var sql = @"INSERT INTO Customers (FirstName, LastName, Email, PhoneNumber)
                            VALUES (@FirstName, @LastName, @Email, @PhoneNumber); SELECT SCOPE_IDENTITY()";

                return await _connection.ExecuteScalarAsync<int>(sql, customer);

                //await _connection.ExecuteAsync(sql, new
                //{
                //    FirstName = customer.FirstName,
                //    LastName = customer.LastName,
                //    Email = customer.Email,
                //    PhoneNumber = customer.PhoneNumber
                //});
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError($"SQL Error inserting customer: {sqlEx.Message}");
                throw new Exception(sqlEx.StackTrace);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error inserting customer: {ex.Message}");
                throw new Exception(ex.StackTrace);
            }
        }
    }
}
