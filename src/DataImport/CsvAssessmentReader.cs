using CsvHelper.Configuration;

namespace OuladEtlEda.DataImport;

public class CsvAssessmentReader : CsvReaderBase<AssessmentCsv>
{
    public CsvAssessmentReader(string path, CsvConfiguration? configuration = null)
        : base(path, configuration)
    {
    }
}