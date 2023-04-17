using DripChip.Domain.Abstractions;
using DripChip.Geo;

namespace DripChip.Domain.Entities;

public class LocationPoint : Point, IEntity<long>
{
    public long Id { get; set; }
    public IList<Animal> ChippedAnimals { get; set; } = new List<Animal>();
    public IList<Visit> AnimalVisits { get; set; } = new List<Visit>();
}