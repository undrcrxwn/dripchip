using System.Linq.Expressions;
using DripChip.Application.Abstractions.Filtering;

namespace DripChip.Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TItem> Where<TItem>(this IQueryable<TItem> items, ISpecification<TItem> specification) =>
        items.Where(specification.ToExpression());

    public static IQueryable<TItem> Where<TItem, TProperty>(this IQueryable<TItem> items,
        Expression<Func<TItem, TProperty>> selector, ISpecification<TProperty> specification)
    {
        var parameter = Expression.Parameter(typeof(TItem));
        var itemPredicate =
            Expression.Lambda<Func<TItem, bool>>(
                Expression.Invoke(
                    expression: specification.ToExpression(),
                    arguments: Expression.Invoke(selector, parameter)),
                parameter);

        return items.Where(itemPredicate);
    }
}