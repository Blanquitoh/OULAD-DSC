using Serilog;

namespace OuladEtlEda;

/// <summary>
/// Provides a basic Serilog logger that writes to the console and a rolling
/// log file.
/// </summary>
public static class Logger
{
    /// <summary>
    /// Gets the configured Serilog logger instance.
    /// </summary>
    public static ILogger Instance { get; }

    static Logger()
    {
        Instance = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }
}
