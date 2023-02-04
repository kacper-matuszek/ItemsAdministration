namespace ItemsAdministration.Common.Application.Abstractions.Queries;

public record PaginatedQuery
(
    int PageSize,
    int PageNumber,
    string? GlobalSearch,
    SortingOptions SortingOptions
) : SortedQuery(SortingOptions);