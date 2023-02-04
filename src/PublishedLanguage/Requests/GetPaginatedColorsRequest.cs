using System.Collections.Generic;
using ItemsAdministration.Common.Shared.Requests;

namespace ItemsAdministration.PublishedLanguage.Requests;

public class GetPaginatedColorsRequest : IPaginatedListFilterRequest
{
    public PagingRequest PagingRequest { get; set; } = new PagingRequest();
    public List<SortedColumnRequest> SortedColumns { get; set; } = new List<SortedColumnRequest>();
}