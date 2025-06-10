using CsvHelper.Configuration;
using OuladEtlEda.DataImport.Models;

namespace OuladEtlEda.DataImport.Readers;

public class CsvAssessmentReader(string path, CsvConfiguration? configuration = null)
    : CsvReaderBase<AssessmentCsv>(path, configuration);