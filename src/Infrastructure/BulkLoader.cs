using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace OuladEtlEda.Infrastructure;

public class BulkLoader
{
    public async Task BulkInsertAsync<T>(DbContext context, IList<T> entities) where T : class
    {
        var entityType = context.Model.FindEntityType(typeof(T));
        var keyProps = entityType?.FindPrimaryKey()?.Properties;
        if (keyProps != null && keyProps.Count > 0)
        {
            var seen = new HashSet<string>();
            var deduped = new List<T>(entities.Count);
            foreach (var entity in entities)
            {
                var key = string.Join('|', keyProps.Select(p => typeof(T).GetProperty(p.Name)!.GetValue(entity)?.ToString()));
                if (seen.Add(key))
                {
                    deduped.Add(entity);
                }
                else
                {
                    Log.Information("Skipping duplicate {EntityType} with key {Key}", typeof(T).Name, key);
                }
            }
            entities = deduped;
        }

        await context.BulkInsertAsync(entities);
    }
}