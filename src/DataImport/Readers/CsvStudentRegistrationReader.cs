using CsvHelper.Configuration;
using OuladEtlEda.DataImport.Models;

namespace OuladEtlEda.DataImport.Readers;

public class CsvStudentRegistrationReader(string path, CsvConfiguration? configuration = null)
    : CsvReaderBase<StudentRegistrationCsv>(path, configuration);