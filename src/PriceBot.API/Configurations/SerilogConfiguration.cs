using Serilog;

namespace PriceBot.API.Configurations;

public static class SerilogConfiguration
{
    public static WebApplicationBuilder AddSerilogConfiguration(this WebApplicationBuilder builder, string path)
    {
        var p = Path.Combine(path, "Logs/log-.txt");
        Log.Logger = new LoggerConfiguration()
            .CreateBootstrapLogger();

        builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            
            // Moved to appsettings.json
            //
            //.MinimumLevel.Information()
            //.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            //.Enrich.WithCorrelationId()
            //.Enrich.WithCorrelationIdHeader()
            //.Enrich.FromLogContext()
            //.WriteTo.Console(
            //    //restrictedToMinimumLevel: LogEventLevel.Information,
            //    outputTemplate: _consoleLogTemplate
            //)
            //.WriteTo.File(
            //    p,
            //    //restrictedToMinimumLevel: LogEventLevel.Information,
            //    rollingInterval: RollingInterval.Day,
            //    retainedFileCountLimit: 10,
            //    outputTemplate: _fileLogTemplate
            //)
        );

        builder.Services.AddHttpContextAccessor(); // For CorrelationId

        return builder;
    }
}
