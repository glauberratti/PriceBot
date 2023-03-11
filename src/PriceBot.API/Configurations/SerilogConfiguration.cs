using Serilog;
using Serilog.Events;

namespace PriceBot.API.Configurations;

public static class SerilogConfiguration
{
    private const string _consoleLogTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}";
    private const string _fileLogTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}";

    public static WebApplicationBuilder AddSerilogConfiguration(this WebApplicationBuilder builder, string path)
    {
        var p = Path.Combine(path, "Logs/log-.txt");
        Log.Logger = new LoggerConfiguration()
            .CreateBootstrapLogger();

        builder.Host.UseSerilog((context, services, configuration) => configuration
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.WithCorrelationId()
            .Enrich.WithCorrelationIdHeader()
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Filter.ByExcluding(l => l.RenderMessage().Contains("https://localhost:7025/"))
            .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information, outputTemplate: _consoleLogTemplate)
            .WriteTo.File(p, restrictedToMinimumLevel: LogEventLevel.Information, rollingInterval: RollingInterval.Infinite, retainedFileCountLimit: 10, outputTemplate: _fileLogTemplate)
        );

        builder.Services.AddHttpContextAccessor(); // For CorrelationId

        return builder;
    }
}
