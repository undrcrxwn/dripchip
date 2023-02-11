using DripChip.Application.Abstractions.Filtering;

namespace DripChip.Application.Filters;

public class DelegateFilter<T> : IFilter<T>
{
    private Func<IQueryable<T>, IQueryable<T>> _filter;

    public DelegateFilter(Func<IQueryable<T>, IQueryable<T>> filter) =>
        _filter = filter;

    public IQueryable<T> Apply(IQueryable<T> items) => _filter(items);
}