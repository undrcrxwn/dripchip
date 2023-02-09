using DripChip.Application.Extensions;
using FluentValidation;

namespace DripChip.Application.Features.AnimalTypes.Commands.Update;

public class UpdateAnimalTypeCommandValidator : AbstractValidator<UpdateAnimalTypeCommand>
{
    public UpdateAnimalTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .AnimalTypeId();

        RuleFor(x => x.Type)
            .NotEmpty();
    }
}