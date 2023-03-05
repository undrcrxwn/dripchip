using System.Linq.Expressions;
using DripChip.Application.Abstractions.Filtering;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Infrastructure.Persistence.Services.Specifications;

public sealed class CaseInsensitiveContainsSpecification : ISpecification<string>
{
    private readonly string _query;
    public CaseInsensitiveContainsSpecification(string query) => _query = query;
    public Expression<Func<string, bool>> ToExpression() => item => EF.Functions.ILike(item, $"%{_query}%");
}