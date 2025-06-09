using CsvHelper.Configuration;

namespace OuladEtlEda.DataImport;

public class CsvStudentInfoReader : CsvReaderBase<StudentInfoCsv>
{
    public CsvStudentInfoReader(string path, CsvConfiguration? configuration = null) : base(path, configuration)
    {
    }
}
