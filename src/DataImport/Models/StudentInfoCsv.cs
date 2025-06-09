using CsvHelper.Configuration.Attributes;

namespace OuladEtlEda.DataImport.Models;

public class StudentInfoCsv
{
    [Name("code_module")] public string CodeModule { get; set; } = null!;
    [Name("code_presentation")] public string CodePresentation { get; set; } = null!;
    [Name("id_student")] public int IdStudent { get; set; }
    [Name("gender")] public string Gender { get; set; } = null!;
    [Name("region")] public string? Region { get; set; }
    [Name("highest_education")] public string HighestEducation { get; set; } = null!;
    [Name("imd_band")] public string? ImdBand { get; set; }
    [Name("age_band")] public string AgeBand { get; set; } = null!;
    [Name("num_of_prev_attempts")] public int NumOfPrevAttempts { get; set; }
    [Name("studied_credits")] public int StudiedCredits { get; set; }
    [Name("disability")] public string Disability { get; set; } = null!;
    [Name("final_result")] public string FinalResult { get; set; } = null!;
}