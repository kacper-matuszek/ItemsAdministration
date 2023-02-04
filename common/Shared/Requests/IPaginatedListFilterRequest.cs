namespace ItemsAdministration.Common.Shared.Requests;

public interface IPaginatedListFilterRequest : ISortedListFilterRequest
{
    PagingRequest PagingRequest { get; set; }
}