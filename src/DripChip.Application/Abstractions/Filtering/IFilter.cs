using DripChip.Application.Extensions;
using DripChip.Application.Filters;

namespace DripChip.Application.Abstractions.Filtering;

/// <summary>
/// Basic abstraction for custom IQueryable filtering.
/// </summary>
/// <typeparam name="TItem">Type of entity to be filtered. Is used to declare IQueryable&lt;TItem&gt; contracts.</typeparam>
public interface IFilter<TItem>
{
    public IQueryable<TItem> Apply(IQueryable<TItem> items);

    /// <summary>
    /// Combines multiple filters into a single one with reversed filter invocation inside.
    /// For example, Combine(a, b, c) would result in delegate filter items => items.Where(a).Where(b).Where(c)
    /// </summary>
    /// <param name="filters">Filters to be combined.</param>
    /// <exception cref="ArgumentException">The minimum count of filters is 2.</exception>
    public static IFilter<TItem> Combine(params IFilter<TItem>[] filters)
    {
        if (filters.Length < 2)
            throw new ArgumentException("Not enough filters to combine.", nameof(filters));

        return new DelegateFilter<TItem>(items => filters
            .Aggregate(items, (previousResult, filter) =>
                previousResult.Where(filter)));
    }
}