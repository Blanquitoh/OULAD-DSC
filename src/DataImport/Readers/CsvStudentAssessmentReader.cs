using CsvHelper.Configuration;
using OuladEtlEda.DataImport.Models;

namespace OuladEtlEda.DataImport.Readers;

public class CsvStudentAssessmentReader : CsvReaderBase<StudentAssessmentCsv>
{
    public CsvStudentAssessmentReader(string path, CsvConfiguration? configuration = null) : base(path, configuration)
    {
    }
}