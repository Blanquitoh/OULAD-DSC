using CsvHelper.Configuration;

namespace OuladEtlEda.DataImport;

public class CsvStudentAssessmentReader : CsvReaderBase<StudentAssessmentCsv>
{
    public CsvStudentAssessmentReader(string path, CsvConfiguration? configuration = null) : base(path, configuration)
    {
    }
}
