# NewRepo

## Dependencies

This project uses Entity Framework Core with the SQL Server provider. These
packages are referenced in `OuladEtlEda.csproj`:

```
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.SqlServer
```

## Generating the Schema

The `OuladContext` class defines the tables from the OULAD dataset. To generate
the schema in a database run the standard EF Core commands:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```
