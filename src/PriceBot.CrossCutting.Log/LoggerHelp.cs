namespace PriceBot.CrossCutting.Log;

public static class LoggerHelp
{
    public static void LogInfo(string info)
    {
        Serilog.Log.Information(info);
    }

    public static void LogError(Exception ex, string error)
    {
        Serilog.Log.Error(ex, error);
    }
}
