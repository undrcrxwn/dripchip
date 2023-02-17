using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using DripChip.Domain.Entities;
using DripChip.Domain.Enumerations;
using FluentValidation;
using Mapster;
using Mediator;
using Microsoft.EntityFrameworkCore;
using ValidationException = DripChip.Application.Exceptions.ValidationException;

namespace DripChip.Application.Features.Animals.Commands;

public static class AddVisit
{
    public sealed record Command(long AnimalId, long LocationPointId) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.AnimalId).AnimalId();
            RuleFor(x => x.LocationPointId).LocationPointId();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context) => _context = context;

        public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var animal =
                await _context.Animals
                    .Include(animal => animal.VisitedLocations)
                    .FirstOrDefaultAsync(animal => animal.Id == request.AnimalId, cancellationToken)
                ?? throw new NotFoundException();

            var locationPoint =
                await _context.LocationPoints.FindAsync(request.LocationPointId)
                ?? throw new NotFoundException();

            if (!animal.VisitedLocations.Any() && animal.ChippingLocation == locationPoint)
                throw new ValidationException(nameof(request.LocationPointId),
                    "The specified location point matches the animal's chipping location point.");

            if (animal.LifeStatus == AnimalLifeStatus.Dead)
                throw new ValidationException(nameof(request.LocationPointId),
                    "The animal's location cannot be changed, since the animal is dead.");

            if (animal.VisitedLocations.Any() && animal.VisitedLocations.Last().LocationPointId == locationPoint.Id)
                throw new ValidationException(nameof(request.LocationPointId),
                    "The specified location point matches the animal's current location point.");

            var visit = new Visit
            {
                Visitor = animal,
                LocationPoint = locationPoint,
                DateTimeOfVisitLocationPoint = DateTimeOffset.UtcNow.Trim(TimeSpan.TicksPerMillisecond)
            };

            animal.VisitedLocations.Add(visit);

            await _context.SaveChangesAsync(cancellationToken);
            return visit.Adapt<Response>();
        }
    }
    
    public sealed record Response(long Id, DateTimeOffset DateTimeOfVisitLocationPoint);
}