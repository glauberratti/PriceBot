using PriceBot.CrossCutting.Settings;

namespace PriceBot.API.Configurations;

public static class SettingsConfiguration
{
    public static IServiceCollection AddSettingsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Settings>(options =>
        {
            options.CurrencyApi.Url = configuration.GetSection("ExchangeRateApi:Url").Value ?? string.Empty;
            options.CurrencyApi.EndPointLatest = configuration.GetSection("ExchangeRateApi:EndPointLatest").Value ?? string.Empty;
            options.CurrencyApi.Key = configuration.GetSection("ExchangeRateApi:Key").Value ?? string.Empty;
            options.RabbitMQConfig.VirtualHost = configuration.GetSection("RabbitMQ:VirtualHost").Value ?? string.Empty;
            options.RabbitMQConfig.HostName = configuration.GetSection("RabbitMQ:HostName").Value ?? string.Empty;
            options.RabbitMQConfig.Port = Convert.ToInt16(configuration.GetSection("RabbitMQ:Port").Value ?? string.Empty);
            options.RabbitMQConfig.UserName = configuration.GetSection("RabbitMQ:UserName").Value ?? string.Empty;
            options.RabbitMQConfig.Password = configuration.GetSection("RabbitMQ:Password").Value ?? string.Empty;
            options.RabbitMQConfig.ProductsReprocessingQueue = configuration.GetSection("RabbitMQ:ProductsReprocessingQueue").Value ?? string.Empty;
        });

        return services;
    }
}
