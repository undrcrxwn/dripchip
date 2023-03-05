using System.Linq.Expressions;

namespace DripChip.Application.Abstractions.Filtering;

/// <summary>
/// Basic abstraction for custom filtering.
/// </summary>
/// <typeparam name="TItem">Type of entity to be filtered. Is used to declare IQueryable&lt;TItem&gt; contracts.</typeparam>
public interface ISpecification<TItem>
{
    public Expression<Func<TItem , bool>> ToExpression();
    public sealed bool IsSatisfiedBy(TItem item) => ToExpression().Compile().Invoke(item);
}