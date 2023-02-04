using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

    protected async Task<TSimpleDto> GetSimpleById<TSimpleDto>(
        string byIdSqlQuery,
        object queryParam,
        CancellationToken cancellationToken)
    {
        var commandDefinition = CreateCommandDefinition(byIdSqlQuery, cancellationToken, queryParam);

        using var connection = SqlFactory.CreateDbConnection(DatabaseOptions.Value.ConnectionString);
        return await connection.QuerySingleOrDefaultAsync<TSimpleDto>(commandDefinition).ConfigureAwait(false);
    }

    protected async Task<TResultDto?> GetByIdWithElements<TResultDto>(
        string byIdSqlQuery,
        object queryParam,
        CancellationToken cancellationToken,
        Action<SqlMapper.GridReader, TResultDto> assignElements)
        where TResultDto : class
    {
        var commandDefinition = CreateCommandDefinition(byIdSqlQuery, cancellationToken, queryParam);

        using var connection = SqlFactory.CreateDbConnection(DatabaseOptions.Value.ConnectionString);
        using var multi = await connection.QueryMultipleAsync(commandDefinition).ConfigureAwait(false);

        var result = await multi.ReadSingleOrDefaultAsync<TResultDto>().ConfigureAwait(false);

        if (result == null)
            return null;

        assignElements(multi, result);

        return result;
    }

    protected async Task<List<TModel>> GetSimpleList<TModel>(
        string listSqlQuery,
        CancellationToken cancellationToken,
        object? queryParam = null)
    {
        var commandDefinition = CreateCommandDefinition(listSqlQuery, cancellationToken, queryParam);

        using var connection = SqlFactory.CreateDbConnection(DatabaseOptions.Value.ConnectionString);

        var items = (await connection
            .QueryAsync<TModel>(commandDefinition)
            .ConfigureAwait(false)).ToList();

        return new List<TModel>(items);
    }

    protected async Task<List<TModel>> GetListWithElements<TModel>(
        string listSqlQuery,
        Action<SqlMapper.GridReader, IEnumerable<TModel>> assignElements,
        CancellationToken cancellationToken,
        object? queryParam = null)
    {
        var commandDefinition = CreateCommandDefinition(listSqlQuery, cancellationToken, queryParam);

        using var connection = SqlFactory.CreateDbConnection(DatabaseOptions.Value.ConnectionString);
        using var multi = await connection.QueryMultipleAsync(commandDefinition).ConfigureAwait(false);

        var items = (await multi.ReadAsync<TModel>().ConfigureAwait(false)).ToList();
        assignElements(multi, items);

        return new List<TModel>(items);
    }

    protected static CommandDefinition CreateCommandDefinition(
        string sqlQuery, CancellationToken cancellationToken, object? queryParam = null) =>
        new CommandDefinition(
            commandText: sqlQuery,
            parameters: queryParam,
            cancellationToken: cancellationToken);
}
