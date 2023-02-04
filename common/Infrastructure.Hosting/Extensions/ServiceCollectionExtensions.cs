using System;
using System.Globalization;
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
using System.Text;
using AutoMapper;
using ItemsAdministration.Common.Application.Abstractions.Interfaces.Providers;
using ItemsAdministration.Common.Infrastructure.Readers.Interfaces;
using ItemsAdministration.Common.Infrastructure.Readers;
using Microsoft.Extensions.Localization;
using ItemsAdministration.Common.Infrastructure.Hosting.Localizations;
using ItemsAdministration.Common.Infrastructure.Hosting.Localizations.Interfaces;
using ItemsAdministration.Common.Infrastructure.Hosting.Formatters.Interfaces;
using ItemsAdministration.Common.Infrastructure.Hosting.Formatters;
using ItemsAdministration.Common.Infrastructure.Providers;
using ItemsAdministration.Common.Infrastructure.ReadModel;
using ItemsAdministration.Common.Infrastructure.ReadModel.Factories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Extensions;

public static class ServiceCollectionExtensions
{
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

    internal static IServiceCollection AddJwtAuthenication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthenticationOptions>(configuration.GetSection(AuthenticationOptions.SectionName));
        services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
        services.ConfigureJwtToken();
        return services;
    }

    internal static IServiceCollection AddPolishCulture(this IServiceCollection services)
    {
        var cultureInfo = new CultureInfo("pl-PL");
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

        return services;
    }

    internal static IServiceCollection AddMapper(this IServiceCollection services, params Assembly[] assemblies)
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddMaps(assemblies);
            mc.AddProfile<QueryProfile>();
        });
        var mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        return services;
    }

    internal static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
        services.AddSingleton<IExceptionResponseFormatterFactory, ExceptionResponseFormatterFactory>();

        return services;
    }

    internal static IServiceCollection AddCommonLocalization(this IServiceCollection services)
    {
        services.AddTransient<IDictionaryJsonFileReader, DictionaryJsonFileReader>();
        services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
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

    internal static IServiceCollection AddCqrs(this IServiceCollection services, params Assembly[] assemblies)
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

        services.AddScoped<IDispatcher, InMemoryDispatcher>();
        services.AddMediatR(appAssemblies);

        return services;
    }

    internal static IServiceCollection AddDapperReadModels(this IServiceCollection services, params Assembly[] assemblies)
    {
        const string readModelAssemblySuffix = "ReadModel";

        var readModelAssemblies = assemblies.Select(a => new
            {
                AssemblyName = a.GetName().Name,
                Assembly = a
            }).Where(a => a.AssemblyName != null && a.AssemblyName.EndsWith(readModelAssemblySuffix))
            .Select(a => a.Assembly)
            .ToArray();

        services.AddReadModels(readModelAssemblies);
        services.AddScoped<ISqlFactory, SqlFactory>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services, params Assembly[] assemblies)
    {
        foreach (var (derivedType, interfaceType) in assemblies.SelectMany(t => t.GetDerivedTypesWithInterfaces<RepositoryAttribute>()))
            services.AddScoped(interfaceType, derivedType);

        return services;
    }

    private static IServiceCollection AddReadModels(this IServiceCollection services, params Assembly[] assemblies)
    {
        foreach (var (derivedType, interfaceType) in assemblies.SelectMany(t => t.GetDerivedTypesWithInterfaces<ReadModelAttribute>()))
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

    private static IServiceCollection ConfigureJwtToken(this IServiceCollection services)
    {
        var authenication = services.BuildServiceProvider().GetRequiredService<IOptions<AuthenticationOptions>>().Value;
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenication.SecretKey)),
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidIssuer = authenication.Issuer
                };
            });

        return services;
    }
}