using System.Linq.Expressions;
using DripChip.Application.Abstractions.Filtering;

namespace DripChip.Application.Filters;

public class ExpressionFilter<T> : IFilter<T>
{
    private readonly Expression<Func<T, bool>> _expression;

    public ExpressionFilter(Expression<Func<T, bool>> expression) =>
        _expression = expression;

    public IQueryable<T> Apply(IQueryable<T> items) =>
        items.Where(_expression);
}