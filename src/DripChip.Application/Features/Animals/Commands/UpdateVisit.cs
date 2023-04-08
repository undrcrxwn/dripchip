using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using FluentValidation;
using Mapster;
using Mediator;
using Microsoft.EntityFrameworkCore;
using ValidationException = DripChip.Application.Exceptions.ValidationException;

namespace DripChip.Application.Features.Animals.Commands;

public static class UpdateVisit
{
    public sealed record Command(
        long Id,
        long VisitedLocationPointId,
        long LocationPointId) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id).AnimalId();
            RuleFor(x => x.VisitedLocationPointId).LocationPointId();
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
                    .FirstOrDefaultAsync(animal => animal.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException();

            var visit =
                animal.VisitedLocations.FirstOrDefault(visit => visit.Id == request.VisitedLocationPointId)
                ?? throw new NotFoundException();

            var newLocationPoint =
                await _context.LocationPoints.FindAsync(request.LocationPointId)
                ?? throw new NotFoundException();

            var sortedVisits = animal.VisitedLocations
                .OrderBy(visitedLocation => visitedLocation.DateTimeOfVisitLocationPoint)
                .ToList();
            
            var visitIndex = sortedVisits.IndexOf(visit);

            var previousLocationPointId = sortedVisits.ElementAtOrDefault(visitIndex - 1)?.LocationPointId;
            var nextLocationPointId = sortedVisits.ElementAtOrDefault(visitIndex + 1)?.LocationPointId;

            if (previousLocationPointId == request.LocationPointId ||
                nextLocationPointId == request.LocationPointId)
                throw new ValidationException(nameof(request.LocationPointId),
                    "The specified location point matches any of the neighbor visited locations.");

            if (visit.LocationPointId == request.LocationPointId)
                throw new ValidationException(nameof(request.VisitedLocationPointId),
                    "The specified location point matches the original location point.");

            if (visitIndex == 0 && request.LocationPointId == animal.ChippingLocationId)
                throw new ValidationException(nameof(request.VisitedLocationPointId),
                    "The specified location point matches the animal's chipping location.");

            visit.LocationPoint = newLocationPoint;

            await _context.SaveChangesAsync(cancellationToken);
            return visit.Adapt<Response>();
        }
    }

    public sealed record Response(
        long Id,
        DateTimeOffset DateTimeOfVisitLocationPoint,
        long LocationPointId);
}