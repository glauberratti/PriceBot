using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PriceBot.API.HealthChecks;

namespace PriceBot.API.Configurations;

public static class HealthChecksConfiguration
{
    public static IServiceCollection AddHealthChecksConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddSqlite(configuration.GetConnectionString("PriceBot")!, name: "DB")
            .AddRabbitMQ(configuration.GetConnectionString("RabbitMQ")!, null, "RabbitMQ", null, null, null)
            .AddCheck<CurrencyApiHealthCheck>("Currency API");
        
        services.AddHealthChecksUI()
            .AddInMemoryStorage();

        return services;
    }

    public static WebApplication UseHealthChecksConfiguration(this WebApplication app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions()
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        
        app.UseHealthChecksUI(options =>
        {
            options.UIPath = "/healthdashboard";
            options.AddCustomStylesheet("health_check.css");
        });

        return app;
    }
}
