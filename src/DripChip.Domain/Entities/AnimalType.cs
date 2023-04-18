using DripChip.Domain.Abstractions;

#pragma warning disable CS8618

namespace DripChip.Domain.Entities;

public class AnimalType : IEntity<long>
{
    public long Id { get; set; }
    public string Type { get; set; }
    public IList<Animal> Animals { get; set; } = new List<Animal>();
}