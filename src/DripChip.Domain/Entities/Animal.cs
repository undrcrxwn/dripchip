using DripChip.Domain.Enumerations;
#pragma warning disable CS8618

namespace DripChip.Domain.Entities;

public class Animal : EntityBase<long>
{
    public IList<AnimalType> AnimalTypes { get; set; } = new List<AnimalType>();

    public float Weight { get; set; }
    public float Length { get; set; }
    public float Height { get; set; }

    public AnimalGender Gender { get; set; }
    public AnimalLifeStatus LifeStatus { get; set; }

    public DateTime ChippingDateTime { get; set; }
    public Account Chipper { get; set; }
    public LocationPoint ChippingLocation { get; set; }
    
    // ReSharper disable once CollectionNeverUpdated.Global
    public IList<LocationPoint> LocationPoints { get; set; } = new List<LocationPoint>();

    public DateTime? DeathDateTime { get; set; }
}