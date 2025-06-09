using CsvHelper.Configuration.Attributes;

namespace OuladEtlEda.DataImport.Models;

public class StudentVleCsv
{
    [Name("id_site")] public int IdSite { get; set; }
    [Name("id_student")] public int IdStudent { get; set; }
    [Name("code_module")] public string CodeModule { get; set; } = null!;
    [Name("code_presentation")] public string CodePresentation { get; set; } = null!;
    [Name("date")] public int? Date { get; set; }
    [Name("sum_click")] public int SumClick { get; set; }
}
