using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Dapper;

namespace ItemsAdministration.Common.Infrastructure.ReadModel.Dapper.Builders;

public class PaginatedQueryBuilder
{
    private readonly StringBuilder _builder;

    public PaginatedQueryBuilder(
        string currentPageTableName = "CurrentPage",
        string mainTableName = "MainTableName")
    {
        _builder = new StringBuilder();
        CurrentPageTableName = GetSqlTemporaryTableName(currentPageTableName);
        MainTableName = mainTableName;
        ContainsPaginatedQuery = false;
    }

    public string CurrentPageTableName { get; }
    public bool ContainsPaginatedQuery { get; private set; }
    public DynamicParameters SqlParameters { get; set; } = new DynamicParameters();
    public string MainTableName { get; }
    public string MainQuery { get; private set; } = string.Empty;

    public PaginatedQueryBuilder SetPaginatedQuery(string sqlQuery, int pageNumber, int pageSize, string? additionalWithQueries = null)
    {
        if (string.IsNullOrWhiteSpace(sqlQuery))
            throw new ArgumentException("Sql query cannot be empty.");

        MainQuery = sqlQuery;

        if (ContainsPaginatedQuery)
            _builder.Clear();

        var internalBuilder = new StringBuilder();
        if (string.IsNullOrEmpty(additionalWithQueries))
        {
            internalBuilder
                .AppendLine("WITH List AS");
        }
        else
        {
            internalBuilder
                .AppendLine(additionalWithQueries)
                .AppendLine(", List AS");
        }

        internalBuilder
            .AppendLine("(")
            .AppendLine(MainQuery)
            .AppendLine("),")
            .AppendLine("FilteredList AS")
            .AppendLine("(")
            .AppendLine("SELECT *")
            .AppendLine("FROM List")
            .AppendLine("/**where**/")
            .AppendLine("),")
            .AppendLine("Total AS")
            .AppendLine("(")
            .AppendLine("SELECT COUNT(*) AS TotalCount")
            .AppendLine("FROM FilteredList")
            .AppendLine(")")
            .AppendLine("SELECT *")
            .AppendLine("FROM FilteredList AS SortableList")
            .AppendLine("CROSS JOIN Total")
            .AppendLine("/**orderby**/");

        var paginatedQuery = GenerateSelectLimitWithOffsetSqlQuery(internalBuilder.ToString(), pageSize, (pageNumber - 1) * pageSize);
        var selectIntoTempTableQuery = GenerateSelectIntoTemporaryTableSqlQuery(paginatedQuery, CurrentPageTableName);

        _builder.Append(selectIntoTempTableQuery);
        ContainsPaginatedQuery = true;

        return this;
    }

    public PaginatedQueryBuilder AddSelectTotalCountQuery()
    {
        if (!ContainsPaginatedQuery)
            throw new InvalidOperationException(
                "Can't add a select total count query without providing a paginated query to start with.");

        var selectFirstQuery = GenerateSelectLimitSqlQuery($"SELECT TotalCount FROM {CurrentPageTableName}", 1);
        return AddQuery($"SELECT COALESCE(({selectFirstQuery}), 0)");
    }

    public PaginatedQueryBuilder AddSelectAllFromCurrentPageQuery()
    {
        if (!ContainsPaginatedQuery)
            throw new InvalidOperationException(
                "Can't add a select all from current page query without providing a paginated query to start with.");

        return AddQuery($"SELECT * FROM {CurrentPageTableName} AS SortableList /**orderby**/");
    }

    public PaginatedQueryBuilder AddQuery(string sqlQuery)
    {
        if (sqlQuery == null)
            throw new ArgumentNullException(nameof(sqlQuery));

        if (!sqlQuery.StartsWith(";"))
            sqlQuery = $";{sqlQuery}";

        _builder
            .AppendLine()
            .AppendLine(sqlQuery);

        return this;
    }

    public string GenerateSelectLimitWithOffsetSqlQuery(string sqlQuery, int count, int offset)
    {

        var builder = new StringBuilder(sqlQuery);
        builder.AppendLine();
        builder.AppendLine($"OFFSET {offset}");
        builder.AppendLine($"FETCH NEXT {count} ROWS ONLY");

        return builder.ToString();
    }

    public string GenerateSelectLimitSqlQuery(string sqlQuery, int count) =>
        sqlQuery + $"{Environment.NewLine}LIMIT {count}";

    public string GenerateSelectIntoTemporaryTableSqlQuery(string sqlQuery, string tempTableName, int? fromClauseIndex = null)
    {
        var fromMatch = Regex.Matches(sqlQuery, @"\s+FROM\s", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        var fromClause = fromClauseIndex is null ? fromMatch.LastOrDefault() : fromMatch[fromClauseIndex.Value];

        if (fromClause is null || !fromClause.Success)
            throw new InvalidOperationException("Sql query does not contains FROM statement");

        return sqlQuery.Insert(fromClause.Index, $"{Environment.NewLine}INTO TEMP {GetSqlTemporaryTableName(tempTableName)}");

    }

    public string GetSqlTemporaryTableName(string tempTableName) =>
        tempTableName.StartsWith("tmp_") ? tempTableName : $"tmp_{tempTableName}";

    public string Build()
    {
        if (!ContainsPaginatedQuery)
            throw new InvalidOperationException(
                "Can't build a paginated sql query without providing one to start with.");

        return _builder.ToString();
    }
}
