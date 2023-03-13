using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using PriceBot.CrossCutting.Settings;
using System.Diagnostics;

namespace PriceBot.API.HealthChecks;

public class CurrencyApiHealthCheck : IHealthCheck
{
    private readonly HttpClient _httpClient;
    private readonly Settings _settings;

    public CurrencyApiHealthCheck(HttpClient httpClient, IOptions<Settings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var watch = new Stopwatch();
        watch.Start();
        var teste = _httpClient.GetAsync(_settings.CurrencyApi.Url.Replace("/live", ""), new CancellationToken()).Result;
        watch.Stop();
        _httpClient.Dispose();

        if (teste.IsSuccessStatusCode)
        {
            if (TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds).TotalSeconds > 1)
            {
                return Task.FromResult(HealthCheckResult.Degraded());
            }
            return Task.FromResult(HealthCheckResult.Healthy());
        }
        else
            return Task.FromResult(HealthCheckResult.Unhealthy());
    }
}
