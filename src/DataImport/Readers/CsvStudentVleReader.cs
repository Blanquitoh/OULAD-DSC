using CsvHelper.Configuration;
using OuladEtlEda.DataImport.Models;

namespace OuladEtlEda.DataImport.Readers;

public class CsvStudentVleReader : CsvReaderBase<StudentVleCsv>
{
    public CsvStudentVleReader(string path, CsvConfiguration? configuration = null) : base(path, configuration)
    {
    }
}