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

    private sealed class Validator : AbstractValidator<Command>
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
                    .Include(animal => animal.Visits)
                    .FirstOrDefaultAsync(animal => animal.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException();

            var newLocationPoint =
                await _context.LocationPoints.FindAsync(request.LocationPointId)
                ?? throw new NotFoundException();

            var visit = animal.Visits.FirstOrDefault(visit => visit.Id == request.VisitedLocationPointId);
            if (visit is null)
                throw new NotFoundException();

            var visitIndex = animal.Visits.IndexOf(visit);

            var previousLocationPointId = animal.Visits.ElementAtOrDefault(visitIndex - 1)?.LocationPointId;
            var nextLocationPointId = animal.Visits.ElementAtOrDefault(visitIndex + 1)?.LocationPointId;
            
            if (previousLocationPointId == request.LocationPointId ||
                nextLocationPointId == request.LocationPointId)
                throw new ValidationException(nameof(request.LocationPointId),
                    "The specified location point matches any of the neighbor visited locations.");

            if (visit.LocationPointId == request.LocationPointId)
                throw new ValidationException(nameof(request.VisitedLocationPointId),
                    "The specified location point matches the original location point.");

            if (animal.Visits.First() == visit && visit.LocationPointId == animal.ChippingLocationId)
                throw new ValidationException(nameof(request.VisitedLocationPointId),
                    "The specified location point matches the animal's chipping location.");

            visit.LocationPoint = newLocationPoint;

            await _context.SaveChangesAsync(cancellationToken);
            return animal.Adapt<Response>();
        }
    }

    public sealed record Response(
        long Id,
        IEnumerable<long> AnimalTypes,
        float Weight,
        float Length,
        float Height,
        string Gender,
        string LifeStatus,
        DateTimeOffset ChippingDateTime,
        int ChipperId,
        long ChippingLocationId,
        IEnumerable<long> VisitedLocations,
        DateTimeOffset? DeathDateTime);
}