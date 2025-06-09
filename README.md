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

## Project Structure

The repository is organized into two top-level directories:

- `src` &mdash; application code
- `tests` &mdash; unit tests

Within `src` the layers are grouped into folders:

- `Domain` &mdash; entity models and validation
- `DataAccess` &mdash; EF Core context and migrations
- `DataImport` &mdash; CSV readers
- `Infrastructure` &mdash; logging and bulk loading helpers
- `Pipeline` &mdash; ETL pipeline implementation
