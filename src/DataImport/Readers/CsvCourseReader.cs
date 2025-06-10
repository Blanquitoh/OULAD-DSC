using CsvHelper.Configuration;
using OuladEtlEda.DataImport.Models;

namespace OuladEtlEda.DataImport.Readers;

public class CsvCourseReader(string path, CsvConfiguration? configuration = null)
    : CsvReaderBase<CourseCsv>(path, configuration);