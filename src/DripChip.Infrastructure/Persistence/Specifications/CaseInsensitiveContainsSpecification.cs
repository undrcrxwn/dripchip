using System.Linq.Expressions;
using DripChip.Application.Abstractions.Specifications;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Infrastructure.Persistence.Specifications;

/// <summary>
/// String specification for querying items that contain a specific string. Case-insensitive.
/// </summary>
public sealed class CaseInsensitiveContainsSpecification : Specification<string>
{
    private readonly string _query;
    
    /// <param name="query">Part of string, item needs to contain to satisfy the query.</param>
    public CaseInsensitiveContainsSpecification(string query) => _query = query;

    /// <remark>
    /// Postgres-specific ILIKE operator is used.
    /// </remark>
    /// <seealso cref="EF.Functions"/>
    public override Expression<Func<string, bool>> ToExpression() => item => EF.Functions.ILike(item, $"%{_query}%");
}