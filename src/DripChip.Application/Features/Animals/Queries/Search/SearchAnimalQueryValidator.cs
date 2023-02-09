using DripChip.Application.Extensions;
using DripChip.Domain.Enumerations;
using FluentValidation;

namespace DripChip.Application.Features.Animals.Queries.Search;

public class SearchAnimalQueryValidator : AbstractValidator<SearchAnimalQuery>
{
    public SearchAnimalQueryValidator()
    {
        When(x => x.ChipperId is not null, () =>
            RuleFor(x => x.ChipperId!.Value)
                .AccountId());
        
        When(x => x.ChippingLocationId is not null, () =>
            RuleFor(x => x.ChippingLocationId!.Value)
                .LocationPointId());

        When(x => x.LifeStatus is not null, () =>
            RuleFor(x => x.LifeStatus!)
                .IsInEnum(typeof(AnimalLifeStatus)));
        
        When(x => x.Gender is not null, () =>
            RuleFor(x => x.Gender!)
                .IsInEnum(typeof(AnimalGender)));
        
        RuleFor(x => x.From)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Size)
            .GreaterThan(0);
    }
}