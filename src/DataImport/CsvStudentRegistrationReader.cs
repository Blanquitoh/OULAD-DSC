using CsvHelper.Configuration;

namespace OuladEtlEda.DataImport;

public class CsvStudentRegistrationReader : CsvReaderBase<StudentRegistrationCsv>
{
    public CsvStudentRegistrationReader(string path, CsvConfiguration? configuration = null) : base(path, configuration)
    {
    }
}
