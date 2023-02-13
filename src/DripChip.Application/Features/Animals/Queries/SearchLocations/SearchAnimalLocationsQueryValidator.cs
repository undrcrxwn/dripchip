using DripChip.Application.Extensions;
using FluentValidation;

namespace DripChip.Application.Features.Animals.Queries.SearchLocations;

public class SearchAnimalLocationsQueryValidator : AbstractValidator<SearchAnimalLocationsQuery>
{
    public SearchAnimalLocationsQueryValidator()
    {
        RuleFor(x => x.Id)
            .AnimalId();
        
        RuleFor(x => x.From)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Size)
            .GreaterThan(0);
    }
}