using DripChip.Domain.Common;

namespace DripChip.Domain.Entities;

public class AnimalType : EntityBase<long>
{
    public string Type { get; set; }
}