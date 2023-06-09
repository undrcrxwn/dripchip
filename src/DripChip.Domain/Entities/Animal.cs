using DripChip.Domain.Abstractions;
using DripChip.Domain.Enumerations;
#pragma warning disable CS8618

namespace DripChip.Domain.Entities;

public class Animal : IEntity<long>
{
    public long Id { get; set; }
    
    public IList<AnimalType> AnimalTypes { get; set; } = new List<AnimalType>();

    public float Weight { get; set; }
    public float Length { get; set; }
    public float Height { get; set; }

    public AnimalGender Gender { get; set; }
    public AnimalLifeStatus LifeStatus { get; set; } = AnimalLifeStatus.Alive;

    public DateTimeOffset ChippingDateTime { get; set; }
    
    public int ChipperId { get; set; }
    public Account Chipper { get; set; }
    
    public long ChippingLocationId { get; set; }
    public LocationPoint ChippingLocation { get; set; }
    
    public IList<Visit> VisitedLocations { get; set; } = new List<Visit>();

    public DateTimeOffset? DeathDateTime { get; set; }
}