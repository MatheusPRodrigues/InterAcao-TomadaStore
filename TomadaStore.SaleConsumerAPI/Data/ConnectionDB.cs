using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TomadaStore.Models.Models;

namespace TomadaStore.SaleConsumerAPI.Data
{
    public class ConnectionDB
    {
        private readonly IMongoCollection<Sale> _saleCollection;

        public ConnectionDB(IOptions<MongoDBSettings> mongoDBSettings)
        {
            IMongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _saleCollection = database.GetCollection<Sale>(mongoDBSettings.Value.CollectionName);
        }

        public IMongoCollection<Sale> GetSaleCollection()
        {
            return _saleCollection;
        }
    }
}
