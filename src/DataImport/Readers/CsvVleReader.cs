using OuladEtlEda.DataImport.Models;
using CsvHelper.Configuration;

namespace OuladEtlEda.DataImport.Readers;

public class CsvVleReader : CsvReaderBase<VleCsv>
{
    public CsvVleReader(string path, CsvConfiguration? configuration = null) : base(path, configuration)
    {
    }
}
