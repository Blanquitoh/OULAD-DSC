using OuladEtlEda.DataImport.Models;
using OuladEtlEda.Domain;

namespace OuladEtlEda.Pipeline.Mappers;

public class CourseCsvMapper : ICsvEntityMapper<CourseCsv, Course>
{
    public Course Map(CourseCsv csv) => new()
    {
        CodeModule = csv.CodeModule,
        CodePresentation = csv.CodePresentation,
        ModulePresentationLength = csv.ModulePresentationLength
    };
}
