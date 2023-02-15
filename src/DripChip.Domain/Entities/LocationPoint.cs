namespace DripChip.Domain.Entities;

public class LocationPoint : EntityBase<long>
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public IList<Animal> ChippedAnimals { get; set; } = new List<Animal>();
    public IList<Visit> AnimalVisits { get; set; } = new List<Visit>();
}