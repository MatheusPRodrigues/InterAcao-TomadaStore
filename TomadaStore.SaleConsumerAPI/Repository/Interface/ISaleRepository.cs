using TomadaStore.Models.Models;

namespace TomadaStore.SaleConsumerAPI.Repository.Interface
{
    public interface ISaleRepository
    {
        public Task PersisitSaleAsync(Sale sale);
    }
}
