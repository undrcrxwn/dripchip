using System.Linq.Expressions;
using DripChip.Application.Abstractions.Filtering;
using DripChip.Application.Filters;
using DripChip.Infrastructure.Persistence.Services.Filters;

namespace DripChip.Infrastructure.Persistence.Services;

public class FilterFactory : IFilterFactory
{
    public IFilter<T> CreateCaseInsensitiveContainsFilter<T>(Expression<Func<T, string>> selector, string? query) => query switch
    {
        not null => new CaseInsensitiveContainsFilter<T>(selector, query),
        _ => new AllFilter<T>()
    };

}