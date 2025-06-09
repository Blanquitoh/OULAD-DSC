using OuladEtlEda.DataImport.Models;
using CsvHelper.Configuration;

namespace OuladEtlEda.DataImport.Readers;

public class CsvStudentVleReader : CsvReaderBase<StudentVleCsv>
{
    public CsvStudentVleReader(string path, CsvConfiguration? configuration = null) : base(path, configuration)
    {
    }
}
