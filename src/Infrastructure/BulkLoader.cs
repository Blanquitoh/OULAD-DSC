using System.Linq.Expressions;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace OuladEtlEda.Infrastructure;

public class BulkLoader
{
    private static readonly Dictionary<Type, List<Delegate>> _getterCache = new();

    private static Func<T, object?> BuildGetter<T>(string propertyName)
    {
        var param = Expression.Parameter(typeof(T), "e");
        var property = Expression.Property(param, propertyName);
        var convert = Expression.Convert(property, typeof(object));
        return Expression.Lambda<Func<T, object?>>(convert, param).Compile();
    }

    public async Task BulkInsertAsync<T>(DbContext context, IList<T> entities) where T : class
    {
        var entityType = context.Model.FindEntityType(typeof(T));
        var keyProps = entityType?.FindPrimaryKey()?.Properties;
        if (keyProps != null && keyProps.Count > 0)
        {
            List<Func<T, object?>> getters;
            lock (_getterCache)
            {
                if (!_getterCache.TryGetValue(typeof(T), out var cached))
                {
                    cached = keyProps
                        .Select(p => (Delegate)BuildGetter<T>(p.Name))
                        .ToList();
                    _getterCache[typeof(T)] = cached;
                }

                getters = cached.Cast<Func<T, object?>>().ToList();
            }

            var seen = new HashSet<string>();
            var deduped = new List<T>(entities.Count);
            foreach (var entity in entities)
            {
                var key = string.Join('|', getters.Select(g => g(entity)?.ToString()));
                if (seen.Add(key))
                    deduped.Add(entity);
                else
                    Log.Information("Skipping duplicate {EntityType} with key {Key}", typeof(T).Name, key);
            }

            entities = deduped;
        }

        await context.BulkInsertAsync(entities);
    }
}