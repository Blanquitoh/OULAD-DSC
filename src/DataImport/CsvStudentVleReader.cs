using CsvHelper.Configuration;

namespace OuladEtlEda.DataImport;

public class CsvStudentVleReader : CsvReaderBase<StudentVleCsv>
{
    public CsvStudentVleReader(string path, CsvConfiguration? configuration = null) : base(path, configuration)
    {
    }
}
