using CsvHelper.Configuration.Attributes;

namespace OuladEtlEda.DataImport;

public class VleCsv
{
    [Name("id_site")] public int IdSite { get; set; }
    [Name("code_module")] public string CodeModule { get; set; } = null!;
    [Name("code_presentation")] public string CodePresentation { get; set; } = null!;
    [Name("activity_type")] public string? ActivityType { get; set; }
    [Name("week_from")] public int? WeekFrom { get; set; }
    [Name("week_to")] public int? WeekTo { get; set; }
}
