using ItemsAdministration.Common.Shared.Enums;

namespace ItemsAdministration.Common.Application.Abstractions.Queries;

public record SortDefinition(SortingOrder SortingOrder, int Order, string SortColumn)
{
    public SortDefinition WithSortColumnName(string sortColumnName) =>
        new(SortingOrder, Order, sortColumnName);
}
