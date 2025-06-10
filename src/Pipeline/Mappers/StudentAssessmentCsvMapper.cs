using Microsoft.EntityFrameworkCore;
using OuladEtlEda.DataAccess;
using OuladEtlEda.DataImport.Models;
using OuladEtlEda.Domain;
using Serilog;

namespace OuladEtlEda.Pipeline.Mappers;

public class StudentAssessmentCsvMapper(CategoricalOrdinalMapper mapper, OuladContext context)
    : ICsvEntityMapper<StudentAssessmentCsv, StudentAssessment>
{
    public StudentAssessment? Map(StudentAssessmentCsv csv)
    {
        var idAssessment = mapper.GetOrAdd("assessment_id", csv.IdAssessment.ToString());
        var assessment = context.Assessments.AsNoTracking().FirstOrDefault(a => a.IdAssessment == idAssessment);
        if (assessment != null)
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