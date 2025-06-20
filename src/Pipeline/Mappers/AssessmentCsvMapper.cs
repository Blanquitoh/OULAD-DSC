using OuladEtlEda.DataImport.Models;
using OuladEtlEda.Domain;

namespace OuladEtlEda.Pipeline.Mappers;

public class AssessmentCsvMapper(CategoricalOrdinalMapper mapper) : ICsvEntityMapper<AssessmentCsv, Assessment>
{
    public Assessment Map(AssessmentCsv csv)
    {
        return new Assessment
        {
            IdAssessment = mapper.GetOrAdd("assessment_id", csv.IdAssessment.ToString()),
            CodeModule = csv.CodeModule,
            CodePresentation = csv.CodePresentation,
            AssessmentType = csv.AssessmentType,
            AssessmentTypeEnum = ParseAssessmentType(csv.AssessmentType),
            AssessmentTypeOrdinal = mapper.GetOrAdd("assessment_type", csv.AssessmentType),
            Date = csv.Date,
            Weight = csv.Weight
        };
    }

    private static AssessmentType ParseAssessmentType(string? value)
    {
        return value?.Trim().ToUpper() switch
        {
            "TMA" => AssessmentType.Tma,
            "CMA" => AssessmentType.Cma,
            _ => AssessmentType.Exam
        };
    }
}