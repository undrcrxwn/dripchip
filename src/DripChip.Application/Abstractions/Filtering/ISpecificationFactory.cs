using System.Linq.Expressions;

namespace DripChip.Application.Abstractions.Filtering;

/// <summary>
/// Abstract filter factory used to create filters usually implemented by the persistence layer.
/// For example, is used to create filters that depend on DBSM-specific features.
/// </summary>
public interface ISpecificationFactory
{
    /// <summary>
    /// Provides item filtering by member value. Only queries those items which have their specified members containing the specified query.
    /// Case-insensitive.
    /// </summary>
    /// <param name="query">Text that an item's string member must contain to satisfy the filter.</param>
    /// <returns>Abstract case-insensitive 'property-contains' filter.</returns>
    public ISpecification<string> CaseInsensitiveContains(string? query);
}