namespace TomadaStore.SaleConsumerAPI.Services.Interface
{
    public interface ISaleConsumerService
    {
        public Task ConsumeApprovalsQueueAsync();
    }
}
