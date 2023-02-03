using Microsoft.Extensions.Configuration;
using System;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Extensions;

public static class ConfigurationBuilderExtensions
{
    private const string ConfigurationDir = "Configurations";

    internal static IConfigurationBuilder AddConfiguration(this IConfigurationBuilder builder)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        builder.AddJsonFile($"./{ConfigurationDir}/appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"./{ConfigurationDir}/appsettings.{environment}.json", optional: true);

        return builder;
    }
}
