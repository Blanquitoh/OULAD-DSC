using CsvHelper.Configuration;
using OuladEtlEda.DataImport.Models;

namespace OuladEtlEda.DataImport.Readers;

public class CsvAssessmentReader : CsvReaderBase<AssessmentCsv>
{
    public CsvAssessmentReader(string path, CsvConfiguration? configuration = null)
        : base(path, configuration)
    {
    }
}