using DripChip.Application.Extensions;
using FluentValidation;

namespace DripChip.Application.Features.Animals.Commands.AddLocation;

public class AddLocationToAnimalCommandValidator : AbstractValidator<AddLocationToAnimalCommand>
{
    public AddLocationToAnimalCommandValidator()
    {
        RuleFor(x => x.AnimalId)
            .AnimalId();

        RuleFor(x => x.LocationPointId)
            .LocationPointId();
    }
}