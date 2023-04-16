using DripChip.Domain.Abstractions;
using DripChip.Geo;

#pragma warning disable CS8618

namespace DripChip.Domain.Entities;

public class AreaPoint : Point
{
    public Area Area { get; set; }
    public long AreaId { get; set; }
    public int SequenceId { get; set; }
}