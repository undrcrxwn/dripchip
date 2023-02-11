using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Filtering;

namespace DripChip.Application.Filters;

public class AllFilter<T> : IFilter<T>
{
    public IQueryable<T> Apply(IQueryable<T> items) => items;
}