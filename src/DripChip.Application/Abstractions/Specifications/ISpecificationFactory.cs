namespace DripChip.Application.Abstractions.Specifications;

/// <summary>
/// Abstract specification factory that is used to create specifications usually implemented by the persistence layer.
/// For example, it is used to perform data queries that depend on DBSM-specific features.
/// </summary>
public interface ISpecificationFactory
{
    /// <summary>
    /// Querying all items that contain the specified query. Case-insensitive.
    /// </summary>
    /// <param name="query">Part of string, item needs to contain to satisfy the query. If null, any item satisfies the query.</param>
    /// <returns>Abstract case-insensitive 'property-contains' filter.</returns>
    public Specification<string> CaseInsensitiveContains(string? query);
}