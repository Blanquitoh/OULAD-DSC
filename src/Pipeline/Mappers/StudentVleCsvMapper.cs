using OuladEtlEda.DataImport.Models;
using OuladEtlEda.Domain;

namespace OuladEtlEda.Pipeline.Mappers;

public class StudentVleCsvMapper : ICsvEntityMapper<StudentVleCsv, StudentVle>
{
    public StudentVle Map(StudentVleCsv csv)
    {
        return new StudentVle
        {
            IdSite = csv.IdSite,
            IdStudent = csv.IdStudent,
            CodeModule = csv.CodeModule,
            CodePresentation = csv.CodePresentation,
            Date = csv.Date,
            SumClick = csv.SumClick
        };
    }
}