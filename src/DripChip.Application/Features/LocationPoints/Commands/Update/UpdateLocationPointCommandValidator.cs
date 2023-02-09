using DripChip.Application.Extensions;
using FluentValidation;

namespace DripChip.Application.Features.LocationPoints.Commands.Update;

public class UpdateLocationPointCommandValidator : AbstractValidator<UpdateLocationPointCommand>
{
    public UpdateLocationPointCommandValidator()
    {
        RuleFor(x => x.Id)
            .PointId();
        
        RuleFor(x => x.Latitude)
            .Latitude();

        RuleFor(x => x.Longitude)
            .Longitude();
    }
}