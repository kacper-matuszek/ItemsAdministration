using Microsoft.EntityFrameworkCore;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Extensions;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder ApplyPostgresOptions<TContext>(
        this DbContextOptionsBuilder builder,
        string connectionString)
        where TContext : DbContext =>
        builder.UseNpgsql(
            connectionString, opt =>
            {
                opt.MigrationsAssembly(typeof(TContext).Assembly.FullName);
                opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            }).UseLowerCaseNamingConvention();
}