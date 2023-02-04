using System.Collections.Generic;
using System.Linq;
using ItemsAdministration.Common.Application.Abstractions.Queries;
using ItemsAdministration.Common.Shared.Enums;
using ItemsAdministration.Common.Shared.Requests;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Extensions;

public static class SortedListFilterRequestExtensions
{
    public static SortingOptions GetSortingOptions(this ISortedListFilterRequest request)
    {
        var keyValuePairs = request.SortedColumns.Select(
                sortedColumn => new KeyValuePair<string, SortingOrder>(
                    sortedColumn.ColumnName,
                    sortedColumn.Order))
            .ToList();

        return new SortingOptions(keyValuePairs);
    }
}