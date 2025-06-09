using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace OuladEtlEda.DataImport.Readers;

public abstract class CsvReaderBase<T>
{
    private readonly CsvConfiguration _configuration;
    private readonly string _path;

    protected CsvReaderBase(string path, CsvConfiguration? configuration = null)
    {
        _path = path;
        _configuration = configuration ?? new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true
        };
    }

    public virtual async IAsyncEnumerable<T> ReadAsync()
    {
        await using var stream = File.OpenRead(_path);
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