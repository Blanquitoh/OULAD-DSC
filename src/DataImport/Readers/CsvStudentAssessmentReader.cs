using CsvHelper.Configuration;
using OuladEtlEda.DataImport.Models;

namespace OuladEtlEda.DataImport.Readers;

public class CsvStudentAssessmentReader(string path, CsvConfiguration? configuration = null)
    : CsvReaderBase<StudentAssessmentCsv>(path, configuration);