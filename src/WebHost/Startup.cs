using System.Reflection;
using ItemsAdministration.Application;
using ItemsAdministration.Application.Abstractions;
using ItemsAdministration.Common.Infrastructure.Hosting;
using ItemsAdministration.Common.Infrastructure.Hosting.Extensions;
using ItemsAdministration.Infrastructure.Api;
using ItemsAdministration.Infrastructure.EF.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ItemsAdministration.WebHost;

public class Startup : BaseStartup
{
    public Startup(WebApplicationBuilder builder)
        : base(builder)
    {
    }

    protected override Assembly ApiLayerAssembly => typeof(ApiAssemblyMarker).Assembly;
    protected override Assembly ApplicationLayerAssembly => typeof(ApplicationAssemblyMarker).Assembly;
    protected override Assembly ApplicationAbstractionLayerAssembly => typeof(ApplicationAbstractionAssemblyMarker).Assembly;

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddPostgres<ItemsAdministrationDbContext>(Configuration);

    }

    protected override void ConfigureApplication(WebApplication app)
    {
        base.ConfigureApplication(app);
        app.UseDatabaseAutoMigration<ItemsAdministrationDbContext>();
    }
}