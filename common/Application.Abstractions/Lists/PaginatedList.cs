using System;
using System.Collections;
using System.Collections.Generic;

namespace ItemsAdministration.Common.Application.Abstractions.Lists;

public class PaginatedList<TModel> : IReadOnlyCollection<TModel>
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int PagesCount => CountPages(PageSize, TotalItemsCount);
    public int TotalItemsCount { get; set; }
    public List<TModel> Elements { get; set; } = new List<TModel>();
    public int Count => Elements.Count;

    public IEnumerator<TModel> GetEnumerator() =>
        Elements.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private static int CountPages(int size, int itemsCount)
        => (int)Math.Ceiling((double)itemsCount / size);
}