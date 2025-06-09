using CsvHelper.Configuration.Attributes;

namespace OuladEtlEda.DataImport.Models;

public class StudentRegistrationCsv
{
    [Name("code_module")] public string CodeModule { get; set; } = null!;
    [Name("code_presentation")] public string CodePresentation { get; set; } = null!;
    [Name("id_student")] public int IdStudent { get; set; }
    [Name("date_registration")] public int? DateRegistration { get; set; }
    [Name("date_unregistration")] public int? DateUnregistration { get; set; }
}
