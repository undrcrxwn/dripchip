using System.Linq.Expressions;
using DripChip.Application.Abstractions.Common;
using DripChip.Infrastructure.Persistence.Services.Filters;

namespace DripChip.Infrastructure.Persistence.Services;

public class FilterFactory : IFilterFactory
{
    public IFilter<T> CreateCaseInsensitiveContainsFilter<T>(Expression<Func<T, string>> selector, string query) =>
        new CaseInsensitiveContainsFilter<T>(selector, query);
}