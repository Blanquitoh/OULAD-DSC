using System.CommandLine;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OuladEtlEda.DataImport;
using OuladEtlEda.Domain.Validators;
using OuladEtlEda.Infrastructure;
using OuladEtlEda.Pipeline;
using OuladEtlEda.DataAccess;
using OuladEtlEda.Eda;
using Serilog;
using Serilog.Events;

namespace OuladEtlEda;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        var modeOption = new Option<string>("--mode", () => "etl", "Execution mode");
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
                .AddJsonFile("logging.json", true)
                .Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .MinimumLevel.Is(logLevel)
                .CreateLogger();

            Log.Information("Execution mode: {Mode}", mode);

            var options = new DbContextOptionsBuilder<OuladContext>()
                .UseSqlServer(connectionString)
                .Options;

            using var context = new OuladContext(options);

            if (mode == "etl")
            {
                var courseReader = new CsvCourseReader(Path.Combine(csvDir, "courses.csv"));
                var assessmentReader = new CsvAssessmentReader(Path.Combine(csvDir, "assessments.csv"));
                var studentInfoReader = new CsvStudentInfoReader(Path.Combine(csvDir, "studentInfo.csv"));
                var registrationReader = new CsvStudentRegistrationReader(Path.Combine(csvDir, "studentRegistration.csv"));
                var studentAssessmentReader = new CsvStudentAssessmentReader(Path.Combine(csvDir, "studentAssessment.csv"));
                var vleReader = new CsvVleReader(Path.Combine(csvDir, "vle.csv"));
                var studentVleReader = new CsvStudentVleReader(Path.Combine(csvDir, "studentVle.csv"));

                var mapper = new CategoricalOrdinalMapper();

                var pipeline = new EtlPipeline(
                    courseReader,
                    assessmentReader,
                    studentInfoReader,
                    registrationReader,
                    studentAssessmentReader,
                    vleReader,
                    studentVleReader,
                    mapper,
                    new CourseValidator(),
                    new AssessmentValidator(),
                    new StudentInfoValidator(),
                    new StudentRegistrationValidator(),
                    new StudentAssessmentValidator(),
                    new VleValidator(),
                    new StudentVleValidator(),
                    new BulkLoader(),
                    context);

                await pipeline.RunAsync();
            }
            else if (mode == "eda")
            {
                BasicEda.Run(context);
            }
        }, modeOption, csvDirOption, connectionStringOption, logLevelOption);

        return await root.InvokeAsync(args);
    }
}