using DripChip.Application.Extensions;
using FluentValidation;

namespace DripChip.Application.Features.Animals.Commands.AddType;

public class AddTypeToAnimalCommandValidator : AbstractValidator<AddTypeToAnimalCommand>
{
    public AddTypeToAnimalCommandValidator()
    {
        RuleFor(x => x.AnimalId)
            .AnimalId();

        RuleFor(x => x.AnimalTypeId)
            .AnimalTypeId();
    }
}