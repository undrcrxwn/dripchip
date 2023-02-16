using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using FluentValidation;
using Mapster;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.Animals.Commands;

public static class RemoveVisit
{
    public sealed record Command(long AnimalId, long VisitId) : IRequest<Response>;
    
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.AnimalId).AnimalId();
            RuleFor(x => x.VisitId).AnimalLocationVisitId();
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
                    .FirstOrDefaultAsync(animal => animal.Id == request.AnimalId, cancellationToken)
                ?? throw new NotFoundException();

            var visit = animal.Visits.FirstOrDefault(visit => visit.Id == request.VisitId);
            if (visit is null)
                throw new NotFoundException();

            animal.Visits.Remove(visit);

            if (animal.Visits.FirstOrDefault()?.LocationPointId == animal.ChippingLocationId)
                animal.Visits.RemoveAt(0);
        
            await _context.SaveChangesAsync(cancellationToken);
            return animal.Adapt<Response>();
        }
    }
    
    public record Response(
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