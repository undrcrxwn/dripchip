using System.Linq.Expressions;

namespace DripChip.Application.Abstractions.Filtering;

/// <summary>
/// Abstract filter factory used to create filters usually implemented by the persistence layer.
/// For example, is used to create filters that depend on DBSM-specific features.
/// </summary>
public interface IFilterFactory
{
    /// <summary>
    /// Provides item filtering by member value. Only queries those items which have their specified members containing the specified query.
    /// Case-insensitive.
    /// </summary>
    /// <param name="selector">
    /// A member-access expression, that is used to specify item's string member,
    /// that filtering should be performed on.
    /// </param>
    /// <param name="query">Text that an item's string member must contain to satisfy the filter.</param>
    /// <typeparam name="TItem">Type of entity to be filtered.</typeparam>
    /// <returns>Abstract case-insensitive 'property-contains' filter.</returns>
    public IFilter<TItem> CreateCaseInsensitiveContainsFilter<TItem>(Expression<Func<TItem, string>> selector, string? query);
}