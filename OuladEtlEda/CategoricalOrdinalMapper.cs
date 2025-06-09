using System.Collections.Generic;

namespace OuladEtlEda;

/// <summary>
/// Maps categorical string values to ordinal integers per column.
/// </summary>
public class CategoricalOrdinalMapper
{
    private readonly Dictionary<string, Dictionary<string, int>> _columns = new();

    /// <summary>
    /// Returns the ordinal value mapped to <paramref name="value"/> for the
    /// given <paramref name="column"/>. If the mapping does not exist it will
    /// be created.
    /// </summary>
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
