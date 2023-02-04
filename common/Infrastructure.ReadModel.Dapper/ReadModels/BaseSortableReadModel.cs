using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using ItemsAdministration.Common.Application.Abstractions.Lists;
using ItemsAdministration.Common.Application.Abstractions.Queries;
using ItemsAdministration.Common.Infrastructure.ReadModel.Dapper.Builders;
using ItemsAdministration.Common.Infrastructure.ReadModel.Factories;
using ItemsAdministration.Common.Shared.Enums;
using Microsoft.Extensions.Options;

namespace ItemsAdministration.Common.Infrastructure.ReadModel.Dapper.ReadModels;

public abstract class BaseSortableReadModel : BaseReadModel
{
    private readonly IReadOnlyDictionary<string, string> _sortingColumnNamesIgnoreCaseMap;

    protected BaseSortableReadModel(ISqlFactory connectionFactory, IOptions<IDatabaseOptions> databaseOptions)
        : base(connectionFactory, databaseOptions)
    {
        _sortingColumnNamesIgnoreCaseMap = CreateIgnoreCaseMap(SortingColumnNamesMap);
    }

    protected abstract string DefaultSortingColumnName { get; }
    protected virtual SortingOrder DefaultSortingOrder => SortingOrder.Ascending;
    protected virtual IReadOnlyDictionary<string, string> SortingColumnNamesMap => new Dictionary<string, string>();

    protected Task<PaginatedList<TListDto>> GetSimplePaginatedList<TPaginatedListQuery, TListDto>(
        TPaginatedListQuery query,
        string listSqlQuery,
        Action<TPaginatedListQuery, SqlBuilder> applyFiltering,
        CancellationToken cancellationToken)
        where TPaginatedListQuery : PaginatedQuery
    {
        var paginatedQueryBuilder = new PaginatedQueryBuilder()
            .SetPaginatedQuery(listSqlQuery, query.PageNumber, query.PageSize)
            .AddSelectTotalCountQuery()
            .AddSelectAllFromCurrentPageQuery();

        return GetPaginatedListWithElements<TPaginatedListQuery, TListDto>(
            query, paginatedQueryBuilder, applyFiltering, (_, _) => { }, cancellationToken);
    }

    protected async Task<PaginatedList<TListDto>> GetPaginatedListWithElements<TPaginatedListQuery, TListDto>(
        TPaginatedListQuery query,
        PaginatedQueryBuilder paginatedQueryBuilder,
        Action<TPaginatedListQuery, SqlBuilder> applyFiltering,
        Action<SqlMapper.GridReader, IEnumerable<TListDto>> assignElements,
        CancellationToken cancellationToken)
        where TPaginatedListQuery : PaginatedQuery
    {
        var sqlQuery = paginatedQueryBuilder.Build();
        var builder = new SqlBuilder();
        paginatedQueryBuilder.SqlParameters.AddDynamicParams(new { PageIndex = query.PageNumber - 1, query.PageNumber, query.PageSize });
        var template = builder.AddTemplate(sqlQuery, paginatedQueryBuilder.SqlParameters);

        applyFiltering(query, builder);
        ApplySorting(query.SortingOptions, builder);

        var commandDefinition = CreateCommandDefinition(template.RawSql, cancellationToken, template.Parameters);

        using var connection = SqlFactory.CreateDbConnection(DatabaseOptions.Value.ConnectionString);
        using var multi = await connection.QueryMultipleAsync(commandDefinition).ConfigureAwait(false);

        // This is a workaround. ReadSingleAsync<int> doesn't work (can't cast int64 to int 32), but ReadSingle<int> works perfectly
        var totalCount = (int)await multi.ReadSingleAsync<long>().ConfigureAwait(false);
        var items = (await multi.ReadAsync<TListDto>().ConfigureAwait(false)).ToList();
        assignElements(multi, items);
        paginatedQueryBuilder.SqlParameters = new DynamicParameters();

        return new PaginatedList<TListDto>
        {
            Elements = items,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalItemsCount = totalCount
        };
    }

    protected virtual void ApplySorting(SortingOptions? sortingOptions, SqlBuilder builder)
    {
        if (sortingOptions == null || sortingOptions.SortingDefinitions.Count == 0)
            sortingOptions = GetDefaultSortingOptions();
        else
            sortingOptions = MapSortingColumnNames(sortingOptions);

        if (sortingOptions.SortingDefinitions.Any())
        {
            var sortedSql = string.Join(
                ", ",
                sortingOptions.SortingDefinitions
                    .OrderBy(sd => sd.Order)
                    .Select(sd => sd.SortingOrder == SortingOrder.Ascending ? sd.SortColumn : $"{sd.SortColumn} DESC"));
            builder.OrderBy(sortedSql);
        }
    }

    protected virtual SortingOptions GetDefaultSortingOptions()
    {
        var options = new List<KeyValuePair<string, SortingOrder>>
        {
            new KeyValuePair<string, SortingOrder>(DefaultSortingColumnName, DefaultSortingOrder)
        };

        return new SortingOptions(options);
    }

    protected SortingOptions MapSortingColumnNames(SortingOptions sortingOptions)
    {
        var sortingDefinitions = sortingOptions.SortingDefinitions
            .Select(MapSortingColumnName);

        return new SortingOptions(sortingDefinitions);
    }

    protected SortDefinition MapSortingColumnName(SortDefinition definition) =>
        _sortingColumnNamesIgnoreCaseMap.TryGetValue(definition.SortColumn, out var databaseColumnName)
            ? definition.WithSortColumnName(databaseColumnName)
            : definition;

    private static IReadOnlyDictionary<string, string> CreateIgnoreCaseMap(IReadOnlyDictionary<string, string> source) =>
        new Dictionary<string, string>(source, StringComparer.InvariantCultureIgnoreCase);
}