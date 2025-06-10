using CsvHelper.Configuration;
using OuladEtlEda.DataImport.Models;

namespace OuladEtlEda.DataImport.Readers;

public class CsvStudentVleReader(string path, CsvConfiguration? configuration = null)
    : CsvReaderBase<StudentVleCsv>(path, configuration);