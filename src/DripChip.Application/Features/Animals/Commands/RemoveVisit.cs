using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.Animals.Commands;

public static class RemoveVisit
{
    public sealed record Command(long AnimalId, long VisitId) : IRequest;
    
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.AnimalId).AnimalId();
            RuleFor(x => x.VisitId).AnimalLocationVisitId();
        }
    }
    
    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context) => _context = context;

        public async ValueTask<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var animal =
                await _context.Animals
                    .Include(animal => animal.VisitedLocations)
                    .FirstOrDefaultAsync(animal => animal.Id == request.AnimalId, cancellationToken)
                ?? throw new NotFoundException();

            var visit = animal.VisitedLocations.FirstOrDefault(visit => visit.Id == request.VisitId);
            if (visit is null)
                throw new NotFoundException();

            animal.VisitedLocations.Remove(visit);

            if (animal.VisitedLocations.FirstOrDefault()?.LocationPointId == animal.ChippingLocationId)
                animal.VisitedLocations.RemoveAt(0);
        
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}