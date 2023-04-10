#pragma warning disable CS8618
namespace DripChip.Domain.Abstractions;

/// <summary>
/// Abstraction containing common entity state (e.g. identity, annotations).
/// </summary>
/// <typeparam name="TIdentifier">Type of entity identifier (primary key).</typeparam>
public interface IEntity<TIdentifier>
{
    public TIdentifier Id { get; set; }
}