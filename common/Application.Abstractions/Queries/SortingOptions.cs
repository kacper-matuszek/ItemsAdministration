using System.Collections.Generic;
using System.Linq;
using ItemsAdministration.Common.Shared.Enums;

namespace ItemsAdministration.Common.Application.Abstractions.Queries;

public class SortingOptions
{
    public SortingOptions(IEnumerable<KeyValuePair<string, SortingOrder>> sortColumns)
    {
        SortingDefinitions =
            sortColumns
            .Select((e, idx) => new SortDefinition(e.Value, idx, e.Key))
            .ToList();
    }

    public SortingOptions(IEnumerable<SortDefinition> sortingDefinitions)
    {
        SortingDefinitions = sortingDefinitions.ToList();
    }

    public List<SortDefinition> SortingDefinitions { get; }
}