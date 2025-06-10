using OuladEtlEda.DataImport.Models;
using OuladEtlEda.Domain;

namespace OuladEtlEda.Pipeline.Mappers;

public class StudentInfoCsvMapper(CategoricalOrdinalMapper mapper) : ICsvEntityMapper<StudentInfoCsv, StudentInfo>
{
    public StudentInfo Map(StudentInfoCsv csv)
    {
        return new StudentInfo
        {
            CodeModule = csv.CodeModule,
            CodePresentation = csv.CodePresentation,
            IdStudent = csv.IdStudent,
            Gender = ParseGender(csv.Gender),
            Region = csv.Region,
            RegionOrdinal = mapper.GetOrAdd("region", csv.Region),
            HighestEducation = ParseEducation(csv.HighestEducation),
            ImdBand = csv.ImdBand,
            ImdBandOrdinal = mapper.GetOrAdd("imd_band", csv.ImdBand),
            AgeBand = ParseAgeBand(csv.AgeBand),
            NumOfPrevAttempts = csv.NumOfPrevAttempts,
            StudiedCredits = csv.StudiedCredits,
            Disability = ParseDisability(csv.Disability),
            FinalResult = ParseFinalResult(csv.FinalResult)
        };
    }

    private static Gender ParseGender(string value)
    {
        return value.Trim().ToUpper() switch
        {
            "M" => Gender.Male,
            "F" => Gender.Female,
            _ => Enum.TryParse<Gender>(value, true, out var g) ? g : Gender.Female
        };
    }

    private static AgeBand ParseAgeBand(string value)
    {
        return value.Trim() switch
        {
            "0-35" => AgeBand.Under35,
            "35-55" => AgeBand.From35To55,
            _ => AgeBand.Over55
        };
    }

    private static Disability ParseDisability(string value)
    {
        return value.Trim().ToUpper() switch
        {
            "N" => Disability.No,
            "Y" => Disability.Yes,
            _ => Disability.No
        };
    }

    private static FinalResult ParseFinalResult(string value)
    {
        return Enum.TryParse<FinalResult>(value.Replace(" ", string.Empty), true, out var r) ? r : FinalResult.Fail;
    }

    private static EducationLevel ParseEducation(string value)
    {
        var v = value.ToLower();
        if (v.StartsWith("no formal")) return EducationLevel.NoFormalQual;
        if (v.StartsWith("lower")) return EducationLevel.LowerThanAlevel;
        if (v.StartsWith("a level")) return EducationLevel.ALevelOrEquivalent;
        if (v.StartsWith("he")) return EducationLevel.HEQualification;
        if (v.StartsWith("post")) return EducationLevel.PostGraduate;
        return EducationLevel.NoFormalQual;
    }
}