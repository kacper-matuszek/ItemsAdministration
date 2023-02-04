using ItemsAdministration.Common.Shared.Enums;

namespace ItemsAdministration.Common.Shared.Requests;

public class SortedColumnRequest
{
    public string ColumnName { get; set; } = string.Empty;

    public SortingOrder Order { get; set; }
}