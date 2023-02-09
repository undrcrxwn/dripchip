using FluentValidation;

namespace DripChip.Application.Features.AnimalTypes.Commands.Create;

public class CreateAnimalTypeCommandValidator : AbstractValidator<CreateAnimalTypeCommand>
{
    public CreateAnimalTypeCommandValidator()
    {
        RuleFor(x => x.Type)
            .NotEmpty();
    }
}