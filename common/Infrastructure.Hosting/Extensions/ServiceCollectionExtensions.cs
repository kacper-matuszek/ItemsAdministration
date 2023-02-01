using System;
using System.Linq;
using System.Reflection;
using ItemsAdministration.Common.Infrastructure.Attributes;
using ItemsAdministration.Common.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Extensions;

public static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddPostgres<TContext>(
        this IServiceCollection services, IConfiguration configuration)
        where TContext : DbContext
    {
        services.AddRepositories(typeof(TContext).Assembly);
        services.AddPostgresDatabaseOptions(configuration);
        services.AddDbContext<TContext>(opt => opt.ApplyPostgresOptions<TContext>(
                                            configuration[$"{PostgresDatabaseOptions.SectionName}:{nameof(PostgresDatabaseOptions.ConnectionString)}"]));
        // EF Core issue related to https://github.com/npgsql/efcore.pg/issues/2000
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services, params Assembly[] assemblies)
    {
        foreach (var (derivedType, interfaceType) in assemblies.SelectMany(t => t.GetDerivedTypesWithInterfaces<RepositoryAttribute>()))
            services.AddScoped(interfaceType, derivedType);

        return services;
    }

    private static IServiceCollection AddPostgresDatabaseOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<PostgresDatabaseOptions>(configuration.GetSection(PostgresDatabaseOptions.SectionName));
        return services;
    }
}