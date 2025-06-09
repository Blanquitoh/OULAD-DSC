using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace OuladEtlEda.Infrastructure;

public class BulkLoader
{
    public async Task BulkInsertAsync<T>(DbContext context, IList<T> entities) where T : class
    {
        await context.BulkInsertAsync(entities);
    }
}