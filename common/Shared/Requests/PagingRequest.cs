namespace ItemsAdministration.Common.Shared.Requests;

public class PagingRequest
{
    public int PageSize { get; set; } = 25;

    public int PageNumber { get; set; } = 1;

    public string? GlobalSearch { get; set; }
}