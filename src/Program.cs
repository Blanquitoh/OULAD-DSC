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
        var csvDirOption = new Option<string>("--csv-dir", () => "C:\\csv",
            "Directory containing CSV files");
        var root = new RootCommand { modeOption, csvDirOption };

        root.SetHandler(async (mode, csvDir) =>
        {
            Log.Information("Execution mode: {Mode}", mode);

            var options = new DbContextOptionsBuilder<OuladContext>()
                .UseSqlServer(ConnectionStrings.Default)
                .Options;

            using var context = new OuladContext(options);

            if (mode == "etl")
            {
                var courseReader = new CsvReader<CourseCsv>(Path.Combine(csvDir, "courses.csv"));
                var assessmentReader = new CsvReader<AssessmentCsv>(Path.Combine(csvDir, "assessments.csv"));
                var studentInfoReader = new CsvReader<StudentInfoCsv>(Path.Combine(csvDir, "studentInfo.csv"));
                var registrationReader = new CsvReader<StudentRegistrationCsv>(Path.Combine(csvDir, "studentRegistration.csv"));
                var studentAssessmentReader = new CsvReader<StudentAssessmentCsv>(Path.Combine(csvDir, "studentAssessment.csv"));
                var vleReader = new CsvReader<VleCsv>(Path.Combine(csvDir, "vle.csv"));
                var studentVleReader = new CsvReader<StudentVleCsv>(Path.Combine(csvDir, "studentVle.csv"));

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
        }, modeOption, csvDirOption);

        return await root.InvokeAsync(args);
    }
}