using DripChip.Application.Extensions;
using FluentValidation;

namespace DripChip.Application.Features.Animals.Commands.RemoveType;

public class RemoveTypeFromAnimalCommandValidator : AbstractValidator<RemoveTypeFromAnimalCommand>
{
    public RemoveTypeFromAnimalCommandValidator()
    {
        RuleFor(x => x.AnimalId)
            .AnimalId();

        RuleFor(x => x.AnimalTypeId)
            .AnimalTypeId();
    }
}