using System;
using ItemsAdministration.Common.Infrastructure.Hosting.Extensions;
using ItemsAdministration.Common.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Factories;

public abstract class BaseDbContextFactory<TDbContext> : IDesignTimeDbContextFactory<TDbContext> 
    where TDbContext : DbContext
{
    public TDbContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("./Configurations/appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"./Configurations/appsettings.{environment}.json", optional: true)
            .Build();

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        var connectionString = GetConnectionString(config);

        var optionsBuilder = new DbContextOptionsBuilder<TDbContext>();
        optionsBuilder.ApplyPostgresOptions<TDbContext>(connectionString);

        return CreateDbContextInstance(optionsBuilder.Options);
    }
    protected abstract TDbContext CreateDbContextInstance(DbContextOptions<TDbContext> options);

    protected virtual string GetConnectionString(IConfigurationRoot config) =>
        config[$"{PostgresDatabaseOptions.SectionName}:{nameof(PostgresDatabaseOptions.ConnectionString)}"];
}