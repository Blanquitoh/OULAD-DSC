using CsvHelper.Configuration;

namespace OuladEtlEda.DataImport;

public class CsvVleReader : CsvReaderBase<VleCsv>
{
    public CsvVleReader(string path, CsvConfiguration? configuration = null) : base(path, configuration)
    {
    }
}
