using MongoDB.Driver;
using TomadaStore.ConsumerAPI.Data;
using TomadaStore.ConsumerAPI.Repository.Intefaces;
using TomadaStore.Models.Models;

namespace TomadaStore.ConsumerAPI.Repository
{
    public class SaleRepository : ISaleRepository
    {
        private readonly ILogger<SaleRepository> _logger;
        private readonly IMongoCollection<Sale> _saleCollection;

        public SaleRepository(
            ILogger<SaleRepository> logger,
            ConnectionDB connectionDB)
        {
            _logger = logger;
            _saleCollection = connectionDB.GetSaleCollection();
        }

        public async Task SaveSaleInCollectionAsync(Sale sale)
        {
            try
            {
                await _saleCollection.InsertOneAsync(sale);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating sale: {ex.Message}");
                throw;
            }
        }

        public async Task SaveSalesInCollectionAsync(List<Sale> sales)
        {
            try
            {
                await _saleCollection.InsertManyAsync(sales);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating sale: {ex.Message}");
                throw;
            }
        }
    }
}
