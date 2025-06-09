using OuladEtlEda.DataImport.Models;
using OuladEtlEda.Domain;

namespace OuladEtlEda.Pipeline.Mappers;

public class StudentRegistrationCsvMapper : ICsvEntityMapper<StudentRegistrationCsv, StudentRegistration>
{
    public StudentRegistration Map(StudentRegistrationCsv csv) => new()
    {
        CodeModule = csv.CodeModule,
        CodePresentation = csv.CodePresentation,
        IdStudent = csv.IdStudent,
        DateRegistration = csv.DateRegistration,
        DateUnregistration = csv.DateUnregistration
    };
}
