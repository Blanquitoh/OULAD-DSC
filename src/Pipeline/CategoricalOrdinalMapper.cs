namespace OuladEtlEda.Pipeline;

public class CategoricalOrdinalMapper
{
    private readonly Dictionary<string, Dictionary<string, int>> _columns = new();

    public int GetOrAdd(string column, string value)
    {
        if (!_columns.TryGetValue(column, out var mappings))
        {
            mappings = new Dictionary<string, int>();
            _columns[column] = mappings;
        }

        if (!mappings.TryGetValue(value, out var ordinal))
        {
            ordinal = mappings.Count;
            mappings[value] = ordinal;
        }

        return ordinal;
    }
}