using DripChip.Domain.Abstractions;

#pragma warning disable CS8618

namespace DripChip.Domain.Entities;

public class AreaPoint
{
    public Area Area { get; set; }
    public long AreaId { get; set; }
    public int SequenceId { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
}