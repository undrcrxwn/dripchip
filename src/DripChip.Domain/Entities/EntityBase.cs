#pragma warning disable CS8618
namespace DripChip.Domain.Entities;

public class EntityBase<T>
{
    public T Id { get; set; }
}