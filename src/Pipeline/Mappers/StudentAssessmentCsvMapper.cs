using Microsoft.EntityFrameworkCore;
using OuladEtlEda.DataAccess;
using OuladEtlEda.DataImport.Models;
using OuladEtlEda.Domain;

namespace OuladEtlEda.Pipeline.Mappers;

public class StudentAssessmentCsvMapper : ICsvEntityMapper<StudentAssessmentCsv, StudentAssessment>
{
    private readonly CategoricalOrdinalMapper _mapper;
    private readonly OuladContext _context;

    public StudentAssessmentCsvMapper(CategoricalOrdinalMapper mapper, OuladContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public StudentAssessment Map(StudentAssessmentCsv csv)
    {
        var idAssessment = _mapper.GetOrAdd("assessment_id", csv.IdAssessment.ToString());
        var assessment = _context.Assessments.AsNoTracking().FirstOrDefault(a => a.IdAssessment == idAssessment);
        if (assessment == null)
            throw new InvalidOperationException($"Assessment {csv.IdAssessment} not found");

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
    }
}
