using Microsoft.Extensions.Configuration;

namespace OuladEtlEda.Infrastructure;

public static class ConnectionStrings
{
    private static readonly IConfigurationRoot Config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: true)
        .AddJsonFile("appsettings.sample.json", optional: true)
        .Build();

    public static string Default => Config.GetConnectionString("Default")!;
}
