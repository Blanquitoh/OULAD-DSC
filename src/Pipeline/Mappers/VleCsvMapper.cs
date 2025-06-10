using OuladEtlEda.DataImport.Models;
using OuladEtlEda.Domain;

namespace OuladEtlEda.Pipeline.Mappers;

public class VleCsvMapper(CategoricalOrdinalMapper mapper) : ICsvEntityMapper<VleCsv, Vle>
{
    public Vle Map(VleCsv csv)
    {
        return new Vle
        {
            IdSite = csv.IdSite,
            CodeModule = csv.CodeModule,
            CodePresentation = csv.CodePresentation,
            ActivityType = csv.ActivityType,
            ActivityTypeOrdinal = mapper.GetOrAdd("activity_type", csv.ActivityType),
            WeekFrom = csv.WeekFrom,
            WeekTo = csv.WeekTo
        };
    }
}