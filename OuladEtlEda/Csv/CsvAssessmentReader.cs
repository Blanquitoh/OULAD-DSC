using CsvHelper.Configuration;

namespace OuladEtlEda.Csv;

public class CsvAssessmentReader : CsvReaderBase<AssessmentCsv>
{
    public CsvAssessmentReader(string path, CsvConfiguration? configuration = null)
        : base(path, configuration)
    {
    }
}
