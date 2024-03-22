using Microsoft.Extensions.Configuration;
// ReSharper disable StringLiteralTypo

namespace Eu.Iamia.ConfigBase;

public class ConfigurationSetup
{
    // NB! requires NuGet package Microsoft.Extensions.Configuration.EnvironmentVariables -> (builder.AddEnvironmentVariables())
    // NB! requires NuGet package Microsoft.Extensions.Configuration.Json -> (builder.AddJsonFile(...))
    // NB! requires NuGet package Microsoft.Extensions.Configuration.CommandLine -> (builder.AddCommandLine(...)

    public static IConfiguration Init(string[]? args)
    {
        // Note: appsettings.json files must be copied to output directory (properties setting)

        var builder = new ConfigurationBuilder();

        builder.AddJsonFile("appsettings.json", true, true);

        var environmentKeys = new[]
        {
            "ASPNETCORE_ENVIRONMENT", // This doesn't work (come for free) in NCrunch/ReSharper tests.
            "WSA_TEST",
            "COMPUTERNAME"
        };
        foreach (var environmentKey in environmentKeys)
        {
            var env = Environment.GetEnvironmentVariable(environmentKey);
            if (env != null) { builder.AddJsonFile($"appsettings.{env}.json", true, true); }

        }

        builder.AddEnvironmentVariables();

        builder.AddCommandLine(args ?? Array.Empty<string>());

        IConfiguration config = builder.Build();

        return config;
    }
}