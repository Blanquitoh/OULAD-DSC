using CsvHelper.Configuration;
using OuladEtlEda.DataImport.Models;

namespace OuladEtlEda.DataImport.Readers;

public class CsvStudentInfoReader(string path, CsvConfiguration? configuration = null)
    : CsvReaderBase<StudentInfoCsv>(path, configuration);