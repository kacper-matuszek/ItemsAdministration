using System.Collections.Generic;

namespace ItemsAdministration.Common.Shared.Responses;

public class ListResponse<TResponse>
{
    public List<TResponse> Elements { get; set; } = new List<TResponse>();
}