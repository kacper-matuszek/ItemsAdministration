namespace ItemsAdministration.Common.Shared.Responses;

public class PaginatedListResponse<TResponse> : ListResponse<TResponse>
{
    public PagingResponse PagingResponse { get; set; } = new PagingResponse();
}