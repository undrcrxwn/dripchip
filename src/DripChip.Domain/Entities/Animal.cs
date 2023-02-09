using DripChip.Domain.Abstractions;
using DripChip.Domain.Common;
using DripChip.Domain.Enumerations;

namespace DripChip.Domain.Entities;

public class Animal : EntityBase<long>
{
    public IList<AnimalType> AnimalTypes { get; private set; } = new List<AnimalType>();

    public float Weight { get; set; }
    public float Length { get; set; }
    public float Height { get; set; }

    public AnimalGender Gender { get; set; }
    public AnimalLifeStatus LifeStatus { get; set; }

    public DateTime ChippingDateTime { get; set; }
    public IAccount Chipper { get; set; }
    public LocationPoint ChippingLocation { get; set; }
    public IList<LocationPoint> LocationPoints { get; private set; } = new List<LocationPoint>();

    public DateTime? DeathDateTime { get; set; }
}