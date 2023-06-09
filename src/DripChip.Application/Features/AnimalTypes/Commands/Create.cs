using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Domain.Entities;
using FluentValidation;
using Mapster;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.AnimalTypes.Commands;

public static class Create
{
    public sealed record Command(string Type) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator() => RuleFor(x => x.Type).NotEmpty();
    }

    internal sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context) => _context = context;

        public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var sameExists = await _context.AnimalTypes
                .AnyAsync(animalType => animalType.Type == request.Type, cancellationToken);

            if (sameExists)
                throw new AlreadyExistsException();
        
            var entity = request.Adapt<AnimalType>();
            await _context.AnimalTypes.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        
            return entity.Adapt<Response>();
        }
    }

    public sealed record Response(long Id, string Type);
}