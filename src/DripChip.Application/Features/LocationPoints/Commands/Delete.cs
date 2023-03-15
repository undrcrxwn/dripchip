using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using DripChip.Domain.Entities;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;
using ValidationException = DripChip.Application.Exceptions.ValidationException;

namespace DripChip.Application.Features.LocationPoints.Commands;

public static class Delete
{
    public sealed record Command(long Id) : IRequest;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator() => RuleFor(x => x.Id).LocationPointId();
    }

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context) => _context = context;

        public async ValueTask<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var locationPoint =
                await _context.LocationPoints
                    .Include(point => point.ChippedAnimals)
                    .Include(point => point.AnimalVisits)
                    .FirstOrDefaultAsync(point => point.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(LocationPoint), request.Id);
        
            if (locationPoint.ChippedAnimals.Any())
                throw new ValidationException(nameof(request.Id), "The specified location is a chipping point for one or more animals.");
        
            if (locationPoint.AnimalVisits.Any())
                throw new ValidationException(nameof(request.Id), "The specified location is a visited point for one or more animals.");

            _context.LocationPoints.Remove(locationPoint);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}