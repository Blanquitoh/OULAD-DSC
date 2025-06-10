using OuladEtlEda.DataAccess;
using OuladEtlEda.DataImport;
using OuladEtlEda.DataImport.Models;
using OuladEtlEda.Domain.Validators;
using OuladEtlEda.Infrastructure;
using OuladEtlEda.Pipeline;

namespace OuladEtlEda.Etl
{
    public static class MainEtl
    {
        public static async Task Run(OuladContext context, String csvDir)
        {
            var courseReader = new CsvReader<CourseCsv>(Path.Combine(csvDir, "courses.csv"));
            var assessmentReader = new CsvReader<AssessmentCsv>(Path.Combine(csvDir, "assessments.csv"));
            var studentInfoReader = new CsvReader<StudentInfoCsv>(Path.Combine(csvDir, "studentInfo.csv"));
            var registrationReader =
                new CsvReader<StudentRegistrationCsv>(Path.Combine(csvDir, "studentRegistration.csv"));
            var studentAssessmentReader =
                new CsvReader<StudentAssessmentCsv>(Path.Combine(csvDir, "studentAssessment.csv"));
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
    }
}