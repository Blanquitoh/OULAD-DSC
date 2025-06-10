using Microsoft.Extensions.Configuration;

namespace OuladEtlEda.Infrastructure;

public static class ConnectionStrings
{
    private static readonly IConfigurationRoot Config =
        new ConfigurationBuilder()
            .SetBasePath(Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..")))
            .AddJsonFile("appsettings.json", false)
            .Build();

    public static string Default => Config.GetConnectionString("Default")!;
}