using System.Linq.Expressions;
using DripChip.Application.Abstractions.Filtering;

namespace DripChip.Application.Filters;

public class ExpressionFilter<T> : IFilter<T>
{
    public readonly Expression<Func<T, bool>> PredicateExpression;

    public ExpressionFilter(Expression<Func<T, bool>> predicateExpression) =>
        PredicateExpression = predicateExpression;

    public IQueryable<T> Apply(IQueryable<T> items) =>
        items.Where(PredicateExpression);
}