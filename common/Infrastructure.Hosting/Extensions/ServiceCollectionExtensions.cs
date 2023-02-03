using System;
using System.Linq;
using System.Reflection;
using ItemsAdministration.Common.Application.Abstractions.Interfaces.Dispatchers;
using ItemsAdministration.Common.Infrastructure.Attributes;
using ItemsAdministration.Common.Infrastructure.Dispatchers;
using ItemsAdministration.Common.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using ItemsAdministration.Common.Shared.Extensions;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCqrs(this IServiceCollection services, Assembly[] assemblies)
    {
        const string appAssemblySuffix = "Application";
        const string appAbstractionsAssemblySuffix = "Application.Abstractions";

        var appAssemblies = assemblies.Select(a => new
        {
            AssemblyName = a.GetName().Name,
            Assembly = a
        }).Where(a => a.AssemblyName != null && (a.AssemblyName.EndsWith(appAssemblySuffix) || a.AssemblyName.EndsWith(appAbstractionsAssemblySuffix)))
        .Select(a => a.Assembly)
        .ToArray();

        if (appAssemblies.Length == 0)
            throw new ArgumentException("Provided assemblies are incorrects. You must provide application and application abstractions assemblies.");

        services.AddSingleton<IDispatcher, InMemoryDispatcher>();
        services.AddMediatR(appAssemblies);

        return services;
    }

    public static IServiceCollection AddPostgres<TContext>(
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