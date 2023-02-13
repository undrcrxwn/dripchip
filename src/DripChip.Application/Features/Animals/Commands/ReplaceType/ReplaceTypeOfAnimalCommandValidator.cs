using DripChip.Application.Extensions;
using FluentValidation;

namespace DripChip.Application.Features.Animals.Commands.ReplaceType;

public class ReplaceTypeOfAnimalCommandValidator : AbstractValidator<ReplaceTypeOfAnimalCommand>
{
    public ReplaceTypeOfAnimalCommandValidator()
    {
        RuleFor(x => x.Id)
            .AnimalId();

        RuleFor(x => x.OldTypeId)
            .AnimalTypeId();

        RuleFor(x => x.NewTypeId)
            .AnimalTypeId();
    }
}