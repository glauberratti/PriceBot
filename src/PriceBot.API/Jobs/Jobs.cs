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
    public async void ProcessProductsValuesJob()
    {
        await _productsProcessing.ProcessUsdValues();
        await _productsProcessing.ProcessEurValues();
    }

    [DisableConcurrentExecution(timeoutInSeconds: 5)]
    [AutomaticRetry(Attempts = 0, LogEvents = false, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    public async void ReprocessValueProductJob()
    {
        await _productsProcessing.ReprocessUsdValueProduct();
        await _productsProcessing.ReprocessEurValueProduct();
    }
}
