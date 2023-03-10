using Microsoft.Extensions.Options;
using PriceBot.CrossCutting.Settings;
using PriceBot.Domain.Queue;

namespace PriceBot.Infra.Data.Queue;

public class ProductsReprocessingQueue : Queue, IProductsReprocessingQueue
{
    private readonly Settings _settings;

    public ProductsReprocessingQueue(IOptions<Settings> settings) : base(settings)
    {
        _settings = settings.Value;
    }

    public void PublishMessage(Guid productId)
    {
        base.PublishMessage<Guid>(_settings.RabbitMQConfig.ProductsReprocessingQueue, productId);
    }

    public Guid? GetMessage(bool removeFromQueue = false)
    {
        return base.GetMessage<Guid>(_settings.RabbitMQConfig.ProductsReprocessingQueue, removeFromQueue);
    }
}
