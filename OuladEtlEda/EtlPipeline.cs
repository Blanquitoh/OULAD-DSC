using System.Collections.Generic;
using System.Threading.Tasks;
using OuladEtlEda.Csv;
using OuladEtlEda.Models;

namespace OuladEtlEda;

/// <summary>
/// Simple ETL pipeline that reads CSV records, maps them to domain entities,
/// validates them and finally inserts them using <see cref="BulkLoader"/>.
/// </summary>
public class EtlPipeline
{
    private readonly CsvAssessmentReader _reader;
    private readonly CategoricalOrdinalMapper _mapper;
    private readonly AssessDomainValidator _validator;
    private readonly BulkLoader _loader;
    private readonly OuladContext _context;

    public EtlPipeline(CsvAssessmentReader reader,
                       CategoricalOrdinalMapper mapper,
                       AssessDomainValidator validator,
                       BulkLoader loader,
                       OuladContext context)
    {
        _reader = reader;
        _mapper = mapper;
        _validator = validator;
        _loader = loader;
        _context = context;
    }

    /// <summary>
    /// Executes the ETL pipeline.
    /// </summary>
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
            Weight = csv.Weight,
        };
    }
}
