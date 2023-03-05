using System.Linq.Expressions;
using DripChip.Application.Abstractions.Filtering;

namespace DripChip.Application.Specifications;

public sealed class AllSpecification<TItem> : ISpecification<TItem>
{
    public Expression<Func<TItem, bool>> ToExpression() => _ => true;
}