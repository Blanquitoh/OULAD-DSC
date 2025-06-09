using CsvHelper.Configuration;
using OuladEtlEda.DataImport.Models;

namespace OuladEtlEda.DataImport.Readers;

public class CsvStudentRegistrationReader : CsvReaderBase<StudentRegistrationCsv>
{
    public CsvStudentRegistrationReader(string path, CsvConfiguration? configuration = null) : base(path, configuration)
    {
    }
}