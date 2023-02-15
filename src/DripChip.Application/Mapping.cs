using DripChip.Domain.Entities;
using Mapster;

namespace DripChip.Application;

public class Mapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Entity to ID
        config.NewConfig<AnimalType, long>()
            .MapWith(point => point.Id);
        
        config.NewConfig<LocationPoint, long>()
            .MapWith(point => point.Id);
        
        config.NewConfig<Visit, long>()
            .MapWith(visit => visit.Id);
        
        // Enumeration to name
        config.NewConfig<Enum, string>()
            .MapWith(enumeration => enumeration.ToString().ToUpper());
    }
}