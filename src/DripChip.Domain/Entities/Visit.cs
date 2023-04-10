using DripChip.Domain.Abstractions;

#pragma warning disable CS8618
namespace DripChip.Domain.Entities;

public class Visit : IEntity<long>
{
    public long Id { get; set; }
    
    public long VisitorId { get; set; }
    public Animal Visitor { get; set; }
    
    public long LocationPointId { get; set; }
    public LocationPoint LocationPoint { get; set; }
    
    public DateTimeOffset DateTimeOfVisitLocationPoint { get; set; }
}