using System.Linq.Expressions;
using DripChip.Application.Abstractions.Specifications;

namespace DripChip.Application.Specifications;

/// <inheritdoc cref="Specification{TItem}"/>
/// <summary>
/// Specification that performs no filtering, so that any provided item is queried.
/// </summary>
public sealed class AllSpecification<TItem> : Specification<TItem>
{
    public override Expression<Func<TItem, bool>> ToExpression() => _ => true;
}