namespace DripChip.Application.Abstractions.Common;

public interface IFilter<T>
{
    public IQueryable<T> Apply(IQueryable<T> items);
}