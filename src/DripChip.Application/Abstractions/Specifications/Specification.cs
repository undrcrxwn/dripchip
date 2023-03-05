using System.Linq.Expressions;

namespace DripChip.Application.Abstractions.Specifications;

/// <summary>
/// Basic abstraction for custom expression-based filtering.
/// </summary>
/// <typeparam name="TItem">Type of entity to be queried.</typeparam>
public abstract class Specification<TItem>
{
    /// <summary>
    /// Compiles the filtering condition into expression.
    /// </summary>
    /// <returns>Expression form of the predicate.</returns>
    public abstract Expression<Func<TItem, bool>> ToExpression();
}