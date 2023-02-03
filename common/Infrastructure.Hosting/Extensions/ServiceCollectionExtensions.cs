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
using Microsoft.AspNetCore.Hosting;
using System.IO;
using ItemsAdministration.Common.Infrastructure.Readers.Interfaces;
using ItemsAdministration.Common.Infrastructure.Readers;
using Microsoft.Extensions.Localization;
using ItemsAdministration.Common.Infrastructure.Hosting.Localizations;
using ItemsAdministration.Common.Infrastructure.Hosting.Localizations.Interfaces;
using ItemsAdministration.Common.Infrastructure.Hosting.Formatters.Interfaces;
using ItemsAdministration.Common.Infrastructure.Hosting.Formatters;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
        services.AddScoped<IExceptionResponseFormatterFactory, ExceptionResponseFormatterFactory>();

        return services;
    }

    public static IServiceCollection AddLocalization(this IServiceCollection services)
    {
        services.AddScoped<IDictionaryJsonFileReader, DictionaryJsonFileReader>();
        services.AddScoped<IStringLocalizerFactory, JsonStringLocalizerFactory>();
        services.AddSingleton<IStringLocalizer, JsonStringLocalizer>();
        services.AddSingleton(sp =>
        {
            const string resourcesDir = "Resources";

            var env = sp.GetRequiredService<IWebHostEnvironment>();
            return Microsoft.Extensions.Options.Options.Create(new JsonReaderOptions(Path.Combine(env.ContentRootPath, resourcesDir)));
        });
        services.AddSingleton<ILocalizationService>(sp =>
        {
            const string messagesFileName = "messages";

            var localizerFactory = sp.GetRequiredService<IStringLocalizerFactory>();
            var localizer = localizerFactory.Create(messagesFileName, string.Empty);

            return new LocalizationService(localizer);
        });

        return services;
    }

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