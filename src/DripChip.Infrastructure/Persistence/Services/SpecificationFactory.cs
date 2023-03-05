using System.Linq.Expressions;
using DripChip.Application.Abstractions.Filtering;
using DripChip.Application.Specifications;
using DripChip.Infrastructure.Persistence.Services.Specifications;

namespace DripChip.Infrastructure.Persistence.Services;

public class SpecificationFactory : ISpecificationFactory
{
    public ISpecification<string> CaseInsensitiveContains(string? query) => query switch
    {
        not null => new CaseInsensitiveContainsSpecification(query),
        _ => new AllSpecification<string>()
    };
}