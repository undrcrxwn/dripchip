namespace DripChip.Application.Abstractions;

public interface IFilter<T>
{
    public IQueryable<T> Apply(IQueryable<T> items);
}