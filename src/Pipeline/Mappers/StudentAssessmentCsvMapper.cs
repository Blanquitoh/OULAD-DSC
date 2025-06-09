using OuladEtlEda.DataImport.Models;
using OuladEtlEda.Domain;

namespace OuladEtlEda.Pipeline.Mappers;

public class StudentAssessmentCsvMapper : ICsvEntityMapper<StudentAssessmentCsv, StudentAssessment>
{
    private readonly CategoricalOrdinalMapper _mapper;

    public StudentAssessmentCsvMapper(CategoricalOrdinalMapper mapper)
    {
        _mapper = mapper;
    }

    public StudentAssessment Map(StudentAssessmentCsv csv)
    {
        return new StudentAssessment
        {
            IdAssessment = _mapper.GetOrAdd("assessment_id", csv.IdAssessment.ToString()),
            IdStudent = csv.IdStudent,
            CodeModule = csv.CodeModule,
            CodePresentation = csv.CodePresentation,
            DateSubmitted = csv.DateSubmitted,
            IsBanked = csv.IsBanked,
            Score = csv.Score
        };
    }
}