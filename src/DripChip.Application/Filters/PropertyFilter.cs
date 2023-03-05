using System.Linq.Expressions;
using DripChip.Application.Abstractions.Filtering;

namespace DripChip.Application.Filters;

public class PropertyFilter<TItem, TProperty> : IFilter<TItem>
{
    private readonly Expression<Func<TItem, TProperty>> _propertySelector;
    private readonly Expression<Func<TProperty, bool>> _propertyPredicate;

    protected PropertyFilter(
        Expression<Func<TItem, TProperty>> propertySelector,
        Expression<Func<TProperty, bool>> propertyPredicate)
    {
        _propertySelector = propertySelector;
        _propertyPredicate = propertyPredicate;
    }

    public IQueryable<TItem> Apply(IQueryable<TItem> items)
    {
        // Lambda parameter of type TItem just like 'x' in 'TItem x => ...'
        var parameter = Expression.Parameter(typeof(TItem));

        // Expression translation for 'TItem item => _propertyPredicate(_propertySelector(item))'
        var itemPredicate =
            Expression.Lambda<Func<TItem, bool>>(
                Expression.Invoke(
                    expression: _propertyPredicate,
                    arguments: Expression.Invoke(_propertySelector, parameter)),
                parameter);

        return items.Where(itemPredicate);
    }
}