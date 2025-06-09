using CsvHelper.Configuration;
using OuladEtlEda.DataImport.Models;

namespace OuladEtlEda.DataImport.Readers;

public class CsvStudentInfoReader : CsvReaderBase<StudentInfoCsv>
{
    public CsvStudentInfoReader(string path, CsvConfiguration? configuration = null) : base(path, configuration)
    {
    }
}