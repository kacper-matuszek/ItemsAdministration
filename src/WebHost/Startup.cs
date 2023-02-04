using System.Reflection;
using ItemsAdministration.Application.Abstractions.Interfaces.Repositories;
using ItemsAdministration.Application.CommandHandlers;
using ItemsAdministration.Common.Infrastructure.Hosting;
using ItemsAdministration.Common.Infrastructure.Hosting.Extensions;
using ItemsAdministration.Infrastructure.Api.Controllers;
using ItemsAdministration.Infrastructure.EF.PostgreSql;
using ItemsAdministration.Infrastructure.ReadModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ItemsAdministration.WebHost;

public class Startup : BaseStartup
{
    public Startup(WebApplicationBuilder builder)
        : base(builder)
    {
    }

    protected override Assembly ApiLayerAssembly => typeof(ItemController).Assembly;
    protected override Assembly ApplicationLayerAssembly => typeof(CreateItemCommandHandler).Assembly;
    protected override Assembly ApplicationAbstractionLayerAssembly => typeof(IItemRepository).Assembly;
    protected override Assembly ReadModelLayerAssembly => typeof(ReadModelAssemblyMarker).Assembly;

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