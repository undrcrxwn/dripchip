using DripChip.Application.Abstractions.Common;

namespace DripChip.Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> Where<T>(this IQueryable<T> items, IFilter<T> filter) =>
        filter.Apply(items);
}