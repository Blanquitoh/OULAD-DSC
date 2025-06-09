using Microsoft.EntityFrameworkCore;
using OuladEtlEda;

var options = new DbContextOptionsBuilder<OuladContext>()
    .UseSqlServer(
        "Data Source=BLANQUITOH-SERV;User ID=Blanquitoh;Password=welc0me;Database=Oulad;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False")
    .Options;

using var db = new OuladContext(options);
Console.WriteLine("Context configured.");