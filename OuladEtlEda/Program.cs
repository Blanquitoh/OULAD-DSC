using System.CommandLine;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OuladEtlEda.Csv;
using Serilog;

namespace OuladEtlEda;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("logging.json", true)
            .Build();
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .CreateLogger();

        var modeOption = new Option<string>("--mode", () => "etl", "Execution mode");
        var csvDirOption = new Option<string>("--csv-dir", "Directory containing CSV files")
        {
            IsRequired = true
        };
        var root = new RootCommand { modeOption, csvDirOption };

        root.SetHandler(async (mode, csvDir) =>
        {
            Log.Information("ETL started");

            var options = new DbContextOptionsBuilder<OuladContext>()
                .UseSqlServer(
                    "Data Source=BLANQUITOH-SERV;User ID=Blanquitoh;Password=welc0me;Database=Oulad;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False")
                .Options;

            using var context = new OuladContext(options);

            if (mode == "etl")
            {
                var reader = new CsvAssessmentReader(Path.Combine(csvDir, "assessments.csv"));
                var mapper = new CategoricalOrdinalMapper();
                var validator = new AssessDomainValidator();
                var loader = new BulkLoader();
                var pipeline = new EtlPipeline(reader, mapper, validator, loader, context);
                await pipeline.RunAsync();
            }
        }, modeOption, csvDirOption);

        return await root.InvokeAsync(args);
    }
}