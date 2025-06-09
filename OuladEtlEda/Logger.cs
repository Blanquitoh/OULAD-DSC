using Serilog;

namespace OuladEtlEda;

public static class Logger
{
    static Logger()
    {
        Instance = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }

    public static ILogger Instance { get; }
}