using System.Threading;
using Dapper;
using ItemsAdministration.Common.Infrastructure.ReadModel.Factories;
using Microsoft.Extensions.Options;

namespace ItemsAdministration.Common.Infrastructure.ReadModel.Dapper.ReadModels;

public abstract class BaseReadModel
{
    protected BaseReadModel(ISqlFactory sqlFactory, IOptions<IDatabaseOptions> databaseOptions)
    {
        DatabaseOptions = databaseOptions;
        SqlFactory = sqlFactory;
    }

    protected ISqlFactory SqlFactory { get; }
    protected IOptions<IDatabaseOptions> DatabaseOptions { get; }

    protected static CommandDefinition CreateCommandDefinition(
        string sqlQuery, CancellationToken cancellationToken, object? queryParam = null) =>
        new CommandDefinition(
            commandText: sqlQuery,
            parameters: queryParam,
            cancellationToken: cancellationToken);
}
