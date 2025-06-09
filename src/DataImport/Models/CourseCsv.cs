using CsvHelper.Configuration.Attributes;

namespace OuladEtlEda.DataImport.Models;

public class CourseCsv
{
    [Name("code_module")] public string CodeModule { get; set; } = null!;
    [Name("code_presentation")] public string CodePresentation { get; set; } = null!;
    [Name("module_presentation_length")] public int ModulePresentationLength { get; set; }
}