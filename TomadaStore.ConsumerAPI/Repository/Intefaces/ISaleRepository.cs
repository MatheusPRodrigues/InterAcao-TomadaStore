using TomadaStore.Models.Models;

namespace TomadaStore.ConsumerAPI.Repository.Intefaces
{
    public interface ISaleRepository
    {
        public Task SaveSaleInCollectionAsync(Sale sale);
        public Task SaveSalesInCollectionAsync(List<Sale> sales);
    }
}
