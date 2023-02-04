using System.Collections.Generic;

namespace ItemsAdministration.Common.Shared.Requests;

public interface ISortedListFilterRequest
{
    List<SortedColumnRequest> SortedColumns { get; set; }
}