using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace OuladEtlEda.DataImport.Readers;

public abstract class CsvReaderBase<T>(string path, CsvConfiguration? configuration = null)
{
    private readonly CsvConfiguration _configuration = configuration ??
                                                       new CsvConfiguration(CultureInfo.InvariantCulture)
                                                       {
                                                           HasHeaderRecord = true
                                                       };

    public virtual async IAsyncEnumerable<T> ReadAsync()
    {
        await using var stream = File.OpenRead(path);
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, _configuration);
        Configure(csv);

        await csv.ReadAsync();
        csv.ReadHeader();

        while (await csv.ReadAsync()) yield return csv.GetRecord<T>();
    }

    protected virtual void Configure(CsvReader csv)
    {
    }
}