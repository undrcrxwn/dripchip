using DripChip.Domain.Abstractions;
using DripChip.Spatial;

namespace DripChip.Domain.Entities;

public class LocationPoint : IEntity<long>
{
    public long Id { get; set; }
    
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    
    public IList<Animal> ChippedAnimals { get; set; } = new List<Animal>();
    public IList<Visit> AnimalVisits { get; set; } = new List<Visit>();

    public Point ToPoint() => new(Longitude, Latitude);
}