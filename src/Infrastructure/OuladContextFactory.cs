using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OuladEtlEda.DataAccess;

namespace OuladEtlEda.Infrastructure;

public class OuladContextFactory : IDesignTimeDbContextFactory<OuladContext>
{
    public OuladContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<OuladContext>()
            .UseSqlServer(ConnectionStrings.Default)
            .Options;

        return new OuladContext(options);
    }
}