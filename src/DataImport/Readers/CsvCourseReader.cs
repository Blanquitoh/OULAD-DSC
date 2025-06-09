using OuladEtlEda.DataImport.Models;
using CsvHelper.Configuration;

namespace OuladEtlEda.DataImport.Readers;

public class CsvCourseReader : CsvReaderBase<CourseCsv>
{
    public CsvCourseReader(string path, CsvConfiguration? configuration = null) : base(path, configuration)
    {
    }
}
