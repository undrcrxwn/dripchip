using DripChip.Application.Abstractions.Specifications;
using DripChip.Application.Specifications;
using DripChip.Infrastructure.Persistence.Specifications;

namespace DripChip.Infrastructure.Persistence.Services;

/// <summary>
/// Concrete persistence layer implementation of the <see cref="ISpecificationFactory"/>.
/// Depends on DBSM-specific features (e.g. Postgres operator ILIKE).
/// </summary>
public sealed class SpecificationFactory : ISpecificationFactory
{
    public Specification<string> CaseInsensitiveContains(string? query) => query switch
    {
        not null => new CaseInsensitiveContainsSpecification(query),
        _ => new AllSpecification<string>()
    };
}