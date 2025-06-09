using OuladEtlEda.DataImport;
using OuladEtlEda.Domain;
using OuladEtlEda.Domain.Validators;
using OuladEtlEda.DataAccess;
using OuladEtlEda.Infrastructure;

namespace OuladEtlEda.Pipeline;

public class EtlPipeline
{
    private readonly OuladContext _context;
    private readonly BulkLoader _loader;
    private readonly CategoricalOrdinalMapper _mapper;
    private readonly CsvAssessmentReader _reader;
    private readonly AssessmentValidator _validator;

    public EtlPipeline(CsvAssessmentReader reader,
        CategoricalOrdinalMapper mapper,
        AssessmentValidator validator,
        BulkLoader loader,
        OuladContext context)
    {
        _reader = reader;
        _mapper = mapper;
        _validator = validator;
        _loader = loader;
        _context = context;
    }

    public async Task RunAsync()
    {
        try
        {
            var entities = new List<Assessment>();
            await foreach (var csv in _reader.ReadAsync())
            {
                var assessment = Map(csv);
                await _validator.ValidateAsync(assessment);
                entities.Add(assessment);
            }

            await _loader.BulkInsertAsync(_context, entities);
        }
        catch (DomainException ex)
        {
            throw new EtlException("Domain validation failed", ex);
        }
    }

    private Assessment Map(AssessmentCsv csv)
    {
        var id = _mapper.GetOrAdd("assessment_id", csv.IdAssessment.ToString());
        return new Assessment
        {
            IdAssessment = id,
            CodeModule = csv.CodeModule,
            CodePresentation = csv.CodePresentation,
            AssessmentType = csv.AssessmentType,
            Date = csv.Date,
            Weight = csv.Weight
        };
    }
}