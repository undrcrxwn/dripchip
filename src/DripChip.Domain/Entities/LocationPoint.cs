using DripChip.Domain.Common;

namespace DripChip.Domain.Entities;

public class LocationPoint : EntityBase<long>
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}