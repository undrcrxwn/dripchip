#pragma warning disable CS8618
namespace DripChip.Domain.Entities;

/// <summary>
/// Abstraction containing common entity state (e.g. identity, annotations).
/// </summary>
/// <typeparam name="TIdentifier">Type of entity identifier (primary key).</typeparam>
public abstract class EntityBase<TIdentifier>
{
    public TIdentifier Id { get; set; }
}