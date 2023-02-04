using System;
using System.Reflection;
using ItemsAdministration.Common.Infrastructure.Hosting.Extensions;
using ItemsAdministration.Common.Infrastructure.Readers.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ItemsAdministration.Common.Infrastructure.Hosting;

public abstract class BaseStartup
{
    private bool _isInitialized;
    private readonly WebApplicationBuilder _builder;

    protected BaseStartup(WebApplicationBuilder builder)
    {
        builder.Configuration.AddConfiguration();

        var provider = builder.Services.BuildServiceProvider();
        Configuration = provider.GetRequiredService<IConfiguration>();

        _builder = builder;
    }

    protected IConfiguration Configuration { get; }

    protected abstract Assembly ApiLayerAssembly { get; }
    protected abstract Assembly ApplicationLayerAssembly { get; }
    protected abstract Assembly ApplicationAbstractionLayerAssembly { get; }

    public WebApplication Initialize()
    {
        if (_isInitialized)
            throw new InvalidOperationException("Cannot again initialize an app.");

        ConfigureHost(_builder.Host);
        ConfigureServices(_builder.Services);

        var app = _builder.Build();
        ConfigureApplication(app);

        _isInitialized = true;

        return app;
    }

    protected virtual void ConfigureHost(ConfigureHostBuilder host)
    {

    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddHttpContextAccessor();
        services.AddControllers().AddApplicationPart(ApiLayerAssembly);
        services.AddCommonLocalization();
        services.AddExceptionHandling();
        services.AddCqrs(new[] { ApplicationLayerAssembly, ApplicationAbstractionLayerAssembly });
        services.AddMapper(new[] { ApiLayerAssembly });
    }

    protected virtual void ConfigureApplication(WebApplication app)
    {
        app.UseExceptionHandler("/error");
        app.UseCors(b =>
        {
            b.AllowAnyHeader();
            b.AllowAnyMethod();
            b.SetIsOriginAllowed(_ => true);
            b.AllowCredentials();
        });
        app.UseExceptionHandling();
        app.UseRouting();
        app.UseEndpoints(e => e.MapControllers());
        app.UseHttpsRedirection();
    }
}