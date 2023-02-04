using System;
using ItemsAdministration.Common.Infrastructure.Hosting.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseDatabaseAutoMigration<TEfContext>(this IApplicationBuilder applicationBuilder)
        where TEfContext : DbContext
    {
        using var scope = applicationBuilder.ApplicationServices.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<TEfContext>>();
        try
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<TEfContext>();
            logger.LogInformation("Running DbContext {0} migrations...", typeof(TEfContext).Name);
            dbContext.Database.Migrate();
        }
        catch (Exception e)
        {
            logger.LogError(exception: e, message: "Error occured during running migrations.");
        }

        return applicationBuilder;
    }

    internal static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder applicationBuilder) =>
        applicationBuilder.UseMiddleware<ExceptionResponseMiddleware>();
}