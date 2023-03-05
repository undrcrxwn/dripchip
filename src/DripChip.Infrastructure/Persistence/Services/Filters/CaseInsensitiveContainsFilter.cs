using System.Linq.Expressions;
using DripChip.Application.Filters;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Infrastructure.Persistence.Services.Filters;

public class CaseInsensitivePropertyContainsFilter<T> : PropertyFilter<T, string>
{
    public CaseInsensitivePropertyContainsFilter(Expression<Func<T, string>> propertySelector, string query)
        : base(propertySelector, property => EF.Functions.ILike(property, $"%{query}%")) { }
}