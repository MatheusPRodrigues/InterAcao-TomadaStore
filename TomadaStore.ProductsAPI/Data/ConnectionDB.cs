using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TomadaStore.Models.Models;

namespace TomadaStore.ProductAPI.Data
{
    public class ConnectionDB
    {
        private readonly IMongoCollection<Product> _productCollection;

        public ConnectionDB(IOptions<MongoDBSettings> mongoDBSettings)
        {
            IMongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _productCollection = database.GetCollection<Product>(mongoDBSettings.Value.CollectionName);
        }

        public IMongoCollection<Product> GetProductCollection()
        {
            return _productCollection;
        }
    }
}
