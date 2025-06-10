using CsvHelper.Configuration;
using OuladEtlEda.DataImport.Readers;

namespace OuladEtlEda.DataImport;

public class CsvReader<T>(string path, CsvConfiguration? configuration = null) : CsvReaderBase<T>(path, configuration);