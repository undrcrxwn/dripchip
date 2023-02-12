using DripChip.Application.Extensions;
using DripChip.Domain.Enumerations;
using FluentValidation;

namespace DripChip.Application.Features.Animals.Commands.Update;

public class UpdateAnimalCommandValidator : AbstractValidator<UpdateAnimalCommand>
{
    public UpdateAnimalCommandValidator()
    {
        RuleFor(x => x.Id)
            .AnimalId();
        
        RuleFor(x => x.Weight)
            .GreaterThan(0);

        RuleFor(x => x.Length)
            .GreaterThan(0);

        RuleFor(x => x.Height)
            .GreaterThan(0);

        RuleFor(x => x.Gender)
            .IsInEnum(typeof(AnimalGender));

        RuleFor(x => x.ChipperId)
            .AccountId();
        
        RuleFor(x => x.ChippingLocationId)
            .LocationPointId();
    }
}