using Hangfire;
using PriceBot.Application.Interfaces;

namespace PriceBot.API.Jobs;

public class Jobs
{
    private readonly IProductsProcessing _productsProcessing;

    public Jobs(IProductsProcessing productsProcessing)
    {
        _productsProcessing = productsProcessing;
    }

    [DisableConcurrentExecution(timeoutInSeconds: 5)]
    [AutomaticRetry(Attempts = 0, LogEvents = false, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    public void ReprocessValueProductJob()
    {
        _ = _productsProcessing.ReprocessUsdValueProduct().Result;
        _ = _productsProcessing.ReprocessEurValueProduct().Result;

    }
}
