using CsvHelper.Configuration;
using OuladEtlEda.DataImport.Models;

namespace OuladEtlEda.DataImport.Readers;

public class CsvVleReader(string path, CsvConfiguration? configuration = null)
    : CsvReaderBase<VleCsv>(path, configuration);