using System.Linq.Expressions;
using DripChip.Application.Abstractions.Common;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Infrastructure.Persistence.Services.Filters;

public class CaseInsensitiveContainsFilter<T> : IFilter<T>
{
    private readonly Expression<Func<T, string>> _selector;
    private readonly string _pattern;

    public CaseInsensitiveContainsFilter(Expression<Func<T, string>> selector, string query)
    {
        _selector = selector;
        _pattern = $"%{query}%";
    }

    public IQueryable<T> Apply(IQueryable<T> items)
    {
        var parameter = Expression.Parameter(typeof(T));

        // Expression form of items.Where(item => EF.Functions.ILike(selector(item), pattern))
        var predicate =
            Expression.Lambda<Func<T, bool>>(
                Expression.Call(
                    instance: null,
                    method: NpgsqlMethodReferences.CaseInsensitiveLikeMethod,
                    Expression.Constant(EF.Functions),
                    Expression.Invoke(_selector, parameter),
                    Expression.Constant(_pattern)),
                parameter);

        return items.Where(predicate);
    }
}