using CsvHelper.Configuration;

namespace OuladEtlEda.DataImport;

public class CsvCourseReader : CsvReaderBase<CourseCsv>
{
    public CsvCourseReader(string path, CsvConfiguration? configuration = null) : base(path, configuration)
    {
    }
}
