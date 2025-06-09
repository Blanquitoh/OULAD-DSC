using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OuladEtlEda;

public class OuladContextFactory : IDesignTimeDbContextFactory<OuladContext>
{
    public OuladContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<OuladContext>()
            .UseSqlServer("Data Source=BLANQUITOH-SERV;User ID=Blanquitoh;Password=welc0me;Database=Oulad;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False")
            .Options;

        return new OuladContext(options);
    }
}

