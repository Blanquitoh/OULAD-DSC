using System.Collections.Generic;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace OuladEtlEda;

/// <summary>
/// Provides helper methods for bulk inserting entities using EF Core BulkExtensions.
/// </summary>
public class BulkLoader
{
    /// <summary>
    /// Performs a bulk insert of the provided entities.
    /// </summary>
    public async Task BulkInsertAsync<T>(DbContext context, IList<T> entities) where T : class
    {
        await context.BulkInsertAsync(entities);
    }
}
