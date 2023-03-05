#pragma warning disable CS8618
namespace DripChip.Domain.Entities;

/// <summary>
/// Abstraction containing common entity state (e.g. identity, annotations).
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class EntityBase<T>
{
    public T Id { get; set; }
}