using MongoDB.Driver;
using TomadaStore.Models.Models;
using TomadaStore.SaleConsumerAPI.Data;
using TomadaStore.SaleConsumerAPI.Repository.Interface;

namespace TomadaStore.SaleConsumerAPI.Repository
{
    public class SaleRepository : ISaleRepository
    {
        private readonly ILogger<SaleRepository> _logger;
        private readonly IMongoCollection<Sale> _saleCollection;

        public SaleRepository(
            ILogger<SaleRepository> logger,
            ConnectionDB connectionDB
        )
        {
            _logger = logger;
            _saleCollection = connectionDB.GetSaleCollection();
        }

        public async Task PersisitSaleAsync(Sale sale)
        {
            try
            {
                await _saleCollection.InsertOneAsync(sale);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurring while persist Sale document in DB: {ex.Message}");
                throw;
            }
        }
    }
}
