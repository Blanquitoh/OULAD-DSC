using CsvHelper.Configuration.Attributes;

namespace OuladEtlEda.DataImport.Models;

public class StudentAssessmentCsv
{
    [Name("id_assessment")] public int IdAssessment { get; set; }
    [Name("id_student")] public int IdStudent { get; set; }
    [Name("code_module")] public string CodeModule { get; set; } = null!;
    [Name("code_presentation")] public string CodePresentation { get; set; } = null!;
    [Name("date_submitted")] public int? DateSubmitted { get; set; }
    [Name("is_banked")] public bool IsBanked { get; set; }
    [Name("score")] public float? Score { get; set; }
}