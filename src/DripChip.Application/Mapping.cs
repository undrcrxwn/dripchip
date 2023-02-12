using DripChip.Domain.Entities;
using Mapster;

namespace DripChip.Application;

public class Mapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AnimalType, long>()
            .MapWith(point => point.Id);
        
        config.NewConfig<LocationPoint, long>()
            .MapWith(point => point.Id);
    }
}