#pragma warning disable CS8618

namespace DripChip.Domain.Entities;

public class AnimalType : EntityBase<long>
{
    public string Type { get; set; }
}