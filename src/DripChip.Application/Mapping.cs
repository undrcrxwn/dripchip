using DripChip.Domain.Entities;
using Mapster;

namespace DripChip.Application;

public sealed class Mapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Entity to ID
        config.NewConfig<EntityBase<long>, long>()
            .MapWith(entity => entity.Id);

        // Enumeration to name
        config.NewConfig<Enum, string>()
            .MapWith(enumeration => enumeration.ToString().ToUpper());
    }
}