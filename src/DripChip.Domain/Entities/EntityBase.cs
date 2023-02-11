namespace DripChip.Domain.Entities;

public class EntityBase<T>
{
    public required T Id { get; set; }
}