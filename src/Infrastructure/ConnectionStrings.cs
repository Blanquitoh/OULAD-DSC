using Microsoft.Extensions.Configuration;

namespace OuladEtlEda.Infrastructure;

public static class ConnectionStrings
{
    private static readonly IConfigurationRoot Config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", true)
        .AddJsonFile("appsettings.sample.json", true)
        .Build();

    public static string Default => Config.GetConnectionString("Default")!;
}