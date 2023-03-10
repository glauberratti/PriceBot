namespace PriceBot.Domain.Queue;

public interface IProductsReprocessingQueue
{
    void PublishMessage(Guid productId);
    Guid? GetMessage(bool removeFromQueue = false);
}
