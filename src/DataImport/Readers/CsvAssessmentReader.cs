using OuladEtlEda.DataImport.Models;
using CsvHelper.Configuration;

namespace OuladEtlEda.DataImport.Readers;

public class CsvAssessmentReader : CsvReaderBase<AssessmentCsv>
{
    public CsvAssessmentReader(string path, CsvConfiguration? configuration = null)
        : base(path, configuration)
    {
    }
}