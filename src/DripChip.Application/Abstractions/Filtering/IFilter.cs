using DripChip.Application.Extensions;
using DripChip.Application.Filters;

namespace DripChip.Application.Abstractions.Filtering;

public interface IFilter<T>
{
    public IQueryable<T> Apply(IQueryable<T> items);
    
    /// <summary>
    /// Combines multiple filters into a single one with reversed filter invocation inside.
    /// For example, Combine(a, b, c) would result in delegate filter items => items.Where(a).Where(b).Where(c)
    /// </summary>
    /// <param name="filters">Filters to be combined.</param>
    /// <exception cref="ArgumentException">The minimum count of filters is 2.</exception>
    public static IFilter<T> Combine(params IFilter<T>[] filters)
    {
        if (filters.Length < 2)
            throw new ArgumentException("Not enough filters to combine.", nameof(filters));

        return new DelegateFilter<T>(items => filters
            .Aggregate(items, (previousResult, filter) =>
                previousResult.Where(filter)));
    }
}