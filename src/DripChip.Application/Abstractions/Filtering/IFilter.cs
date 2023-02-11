using System.Linq.Expressions;
using System.Reflection;
using DripChip.Application.Filters;

namespace DripChip.Application.Abstractions.Filtering;

public interface IFilter<T>
{
    // Method info of IFilter<T>.Apply(IQueryable<T>)
    private static readonly MethodInfo FilterApplyMethodInfo = typeof(IFilter<T>).GetMethod(nameof(Apply))!;

    public IQueryable<T> Apply(IQueryable<T> items);

    /// <summary>
    /// Combines multiple filters into a single one with reversed filter invocation expression inside.
    /// Thus, Combine(a, b, c) would result in the expression filter x => c.Apply(b.Apply(a.Apply(x))).
    /// </summary>
    /// <param name="filters">Filters to be combine.</param>
    /// <exception cref="ArgumentException">The minimum count of filters is 2.</exception>
    public static IFilter<T> Combine(params IFilter<T>[] filters)
    {
        if (filters.Length < 2)
            throw new ArgumentException("Not enough filters to combine.", nameof(filters));

        // Lambda parameter of type T just like 'x' in 'T x => ...'
        var parameter = Expression.Parameter(typeof(T));

        // Originally, invocationExpression is just 'x'
        Expression invocationExpression = parameter;
        // Then it gets wrapped with 'filter.Apply(...)' calls
        foreach (var filter in filters.Reverse())
        {
            // Expression translation for 'filter.Apply(<invocationExpression goes here>)'
            var applyExpression = Expression.Call(Expression.Constant(filter), FilterApplyMethodInfo, invocationExpression);
            invocationExpression = Expression.Invoke(applyExpression, parameter);
        }

        // Started with 'filters = { a, b, c }' where 'a' is filter 'x => ...' 
        // Ended up with 'x => c.Apply(a.Apply(b.Apply(x)))'
        var reversedFilterInvocationExpression = Expression.Lambda<Func<T, bool>>(invocationExpression, parameter);
        return new ExpressionFilter<T>(reversedFilterInvocationExpression);
    }
}