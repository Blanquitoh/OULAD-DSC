using OuladEtlEda.DataImport.Models;
using OuladEtlEda.Domain;

namespace OuladEtlEda.Pipeline.Mappers;

public class AssessmentCsvMapper : ICsvEntityMapper<AssessmentCsv, Assessment>
{
    private readonly CategoricalOrdinalMapper _mapper;

    public AssessmentCsvMapper(CategoricalOrdinalMapper mapper)
    {
        _mapper = mapper;
    }

    public Assessment Map(AssessmentCsv csv)
    {
        return new Assessment
        {
            IdAssessment = _mapper.GetOrAdd("assessment_id", csv.IdAssessment.ToString()),
            CodeModule = csv.CodeModule,
            CodePresentation = csv.CodePresentation,
            AssessmentType = csv.AssessmentType,
            AssessmentTypeOrdinal = csv.AssessmentType == null
                ? null
                : _mapper.GetOrAdd("assessment_type", csv.AssessmentType),
            Date = csv.Date,
            Weight = csv.Weight
        };
    }
}