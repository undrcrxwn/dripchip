using DripChip.Application.Features.Animals.Queries.Search;
using DripChip.Domain.Entities;
using Mapster;

namespace DripChip.Application.Features.Animals;

public class Mapping : IRegister
{
   public void Register(TypeAdapterConfig config)
   {
      config.NewConfig<Animal, SearchAnimalResponse>()
         .Map(
            dto => dto.AnimalTypes,
            entity => entity.AnimalTypes
               .Select(animalType => animalType.Id))
         .Map(
            dto => dto.VisitedLocations,
            entity => entity.LocationPoints
               .Select(locationPoint => locationPoint.Id));
   }
}