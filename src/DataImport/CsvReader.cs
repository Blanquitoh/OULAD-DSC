using CsvHelper.Configuration;
using OuladEtlEda.DataImport.Readers;

namespace OuladEtlEda.DataImport;

public class CsvReader<T> : CsvReaderBase<T>
{
    public CsvReader(string path, CsvConfiguration? configuration = null) : base(path, configuration)
    {
    }
}
