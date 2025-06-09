using OuladEtlEda.DataImport.Models;
using OuladEtlEda.Domain;
using OuladEtlEda.Pipeline;

namespace OuladEtlEda.Pipeline.Mappers;

public class VleCsvMapper : ICsvEntityMapper<VleCsv, Vle>
{
    private readonly CategoricalOrdinalMapper _mapper;

    public VleCsvMapper(CategoricalOrdinalMapper mapper)
    {
        _mapper = mapper;
    }

    public Vle Map(VleCsv csv) => new()
    {
        IdSite = csv.IdSite,
        CodeModule = csv.CodeModule,
        CodePresentation = csv.CodePresentation,
        ActivityType = csv.ActivityType,
        ActivityTypeOrdinal = csv.ActivityType == null ? null : _mapper.GetOrAdd("activity_type", csv.ActivityType),
        WeekFrom = csv.WeekFrom,
        WeekTo = csv.WeekTo
    };
}
