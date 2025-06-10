using OuladEtlEda.DataImport.Models;
using OuladEtlEda.Domain;
using Serilog;

namespace OuladEtlEda.Pipeline.Mappers;

public class StudentAssessmentCsvMapper(
    CategoricalOrdinalMapper mapper,
    IReadOnlyDictionary<int, Assessment> assessments)
    : ICsvEntityMapper<StudentAssessmentCsv, StudentAssessment>
{
    public StudentAssessment? Map(StudentAssessmentCsv csv)
    {
        var idAssessment = mapper.GetOrAdd("assessment_id", csv.IdAssessment.ToString());
        if (assessments.TryGetValue(idAssessment, out var assessment))
            return new StudentAssessment
            {
                IdAssessment = idAssessment,
                IdStudent = csv.IdStudent,
                CodeModule = assessment.CodeModule,
                CodePresentation = assessment.CodePresentation,
                DateSubmitted = csv.DateSubmitted,
                IsBanked = csv.IsBanked,
                Score = csv.Score
            };

        Log.Warning("Assessment {AssessmentId} not found for student {StudentId}", csv.IdAssessment, csv.IdStudent);
        return null;
    }
}