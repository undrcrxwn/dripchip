using DripChip.Domain.Abstractions;

#pragma warning disable CS8618

namespace DripChip.Domain.Entities;

public class Area : IEntity<long>
{
    public long Id { get; set; }
    public string Name { get; set; }
    public IList<AreaPoint> AreaPoints { get; set; } = new List<AreaPoint>();
}