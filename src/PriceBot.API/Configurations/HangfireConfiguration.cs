using Hangfire;
using Hangfire.Storage.SQLite;

namespace PriceBot.API.Configurations;

public static class HangfireConfiguration
{
    public static IServiceCollection AddHangfireConfiguration(this IServiceCollection services)
    {
        services.AddHangfire(options => {
            options.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSQLiteStorage("Hangfire.db");
        });
        services.AddHangfireServer();

        return services;
    }

    public static WebApplication UseHangfireConfiguration(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseHangfireDashboard("/hangfire");
        RecurringJob.AddOrUpdate<Jobs.Jobs>("process-products", x => x.ProcessProductsValuesJob(), Cron.Daily(1));
        RecurringJob.AddOrUpdate<Jobs.Jobs>("reprocess-products", x => x.ReprocessValueProductJob(), "* */3 * * *");
        return app;
    }
}
