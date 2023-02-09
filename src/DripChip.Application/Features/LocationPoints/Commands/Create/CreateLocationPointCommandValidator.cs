using DripChip.Application.Extensions;
using FluentValidation;

namespace DripChip.Application.Features.LocationPoints.Commands.Create;

public class CreateLocationPointCommandValidator : AbstractValidator<CreateLocationPointCommand>
{
    public CreateLocationPointCommandValidator()
    {
        RuleFor(x => x.Latitude)
            .Latitude();

        RuleFor(x => x.Longitude)
            .Longitude();
    }
}