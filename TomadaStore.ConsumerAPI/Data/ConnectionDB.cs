using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TomadaStore.Models.Models;

namespace TomadaStore.ConsumerAPI.Data
{
    public class ConnectionDB
    {
        private readonly IMongoCollection<Sale> _saleCollection;

        public ConnectionDB(IOptions<MongoDBSettings> options)
        {
            IMongoClient client = new MongoClient(options.Value.ConnectionURI);
            IMongoDatabase mongoDatabase = client.GetDatabase(options.Value.DatabaseName);
            _saleCollection = mongoDatabase.GetCollection<Sale>(options.Value.CollectionName);
        }

        public IMongoCollection<Sale> GetSaleCollection()
        {
            return _saleCollection;
        }
    }
}
