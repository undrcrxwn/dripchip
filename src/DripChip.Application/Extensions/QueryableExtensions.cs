using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Filtering;

namespace DripChip.Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> Where<T>(this IQueryable<T> items, IFilter<T> filter) =>
        filter.Apply(items);
}