using TomadaStore.Models.Models;

namespace TomadaStore.PaymentAPI.Services.Interfaces
{
    public interface IPaymentService
    {
        public Task ProcessOrderSalesQueueAsync();
        public Task AddingQueueApprovedSales(Sale sale);
    }
}
