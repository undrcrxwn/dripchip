using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;
using ValidationException = DripChip.Application.Exceptions.ValidationException;

namespace DripChip.Application.Features.Animals.Commands;

public static class Delete
{
    public sealed record Command(long Id) : IRequest;
    
    private sealed class Validator : AbstractValidator<Command>
    {
        public Validator() => RuleFor(x => x.Id).AnimalId();
    }
    
    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context) => _context = context;

        public async ValueTask<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var animal =
                await _context.Animals
                    .Include(animal => animal.Visits)
                    .FirstOrDefaultAsync(animal => animal.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException();

            if (animal.Visits.Any())
                throw new ValidationException(nameof(request.Id), "The specified animal has moved away from its chipping location.");

            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}