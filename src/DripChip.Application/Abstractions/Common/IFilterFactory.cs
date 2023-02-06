using System.Linq.Expressions;

namespace DripChip.Application.Abstractions.Common;

public interface IFilterFactory
{
    public IFilter<T> CreateCaseInsensitiveContainsFilter<T>(Expression<Func<T, string>> selector, string query);
}