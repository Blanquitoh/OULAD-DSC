using Microsoft.EntityFrameworkCore;
using OuladEtlEda;

var options = new DbContextOptionsBuilder<OuladContext>()
    .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Oulad;Trusted_Connection=True;")
    .Options;

using var db = new OuladContext(options);
Console.WriteLine("Context configured.");
