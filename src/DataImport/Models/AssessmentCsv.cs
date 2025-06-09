using CsvHelper.Configuration.Attributes;

namespace OuladEtlEda.DataImport.Models;

public class AssessmentCsv
{
    [Name("id_assessment")] public int IdAssessment { get; set; }

    [Name("code_module")] public string CodeModule { get; set; } = null!;

    [Name("code_presentation")] public string CodePresentation { get; set; } = null!;

    [Name("assessment_type")] public string? AssessmentType { get; set; }

    [Name("date")] public int? Date { get; set; }

    [Name("weight")] public decimal Weight { get; set; }
}