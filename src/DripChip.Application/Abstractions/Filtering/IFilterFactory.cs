using System.Linq.Expressions;

namespace DripChip.Application.Abstractions.Filtering;

public interface IFilterFactory
{
    public IFilter<T> CreateCaseInsensitiveContainsFilter<T>(Expression<Func<T, string>> selector, string? query);
}