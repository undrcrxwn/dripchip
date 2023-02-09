#pragma warning disable CS8618
namespace DripChip.Domain.Common;

public class EntityBase<T>
{
    public T Id { get; set; }
}