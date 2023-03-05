using DripChip.Application.Abstractions.Filtering;

namespace DripChip.Application.Filters;

/// <summary>
/// Applies no filtering to the provided item collection.
/// </summary>
/// <typeparam name="TItem"></typeparam>
public sealed class AllFilter<TItem> : IFilter<TItem>
{
    public IQueryable<TItem> Apply(IQueryable<TItem> items) => items;
}