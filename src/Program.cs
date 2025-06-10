using System.CommandLine;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OuladEtlEda.DataAccess;
using OuladEtlEda.Eda;
using OuladEtlEda.Etl;
using OuladEtlEda.Infrastructure;
using Serilog;
using Serilog.Events;

namespace OuladEtlEda;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        var modeOption = new Option<ExecutionMode>("--mode", () => ExecutionMode.Eda,
            "Execution mode (Etl or Eda)");
        var csvDirOption = new Option<string>("--csv-dir", () => "C:\\csv",
            "Directory containing CSV files");
        var connectionStringOption = new Option<string>("--connection-string",
            () => ConnectionStrings.Default,
            "Database connection string");
        var logLevelOption = new Option<LogEventLevel>("--log-level",
            () => LogEventLevel.Information,
            "Serilog minimum log level");

        var root = new RootCommand
        {
            modeOption,
            csvDirOption,
            connectionStringOption,
            logLevelOption
        };

        root.SetHandler(async (mode, csvDir, connectionString, logLevel) =>
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("logging.json", false)
                .Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .MinimumLevel.Is(logLevel)
                .CreateLogger();

            Log.Information("Execution mode: {Mode}", mode);

            var options = new DbContextOptionsBuilder<OuladContext>()
                .UseSqlServer(connectionString)
                .Options;

            await using var context = new OuladContext(options);

            switch (mode)
            {
                case ExecutionMode.Etl:
                    await MainEtl.Run(context, csvDir);
                    break;
                case ExecutionMode.Eda:
                    ExtendedEda.Run(context);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }, modeOption, csvDirOption, connectionStringOption, logLevelOption);

        return await root.InvokeAsync(args);
    }
}