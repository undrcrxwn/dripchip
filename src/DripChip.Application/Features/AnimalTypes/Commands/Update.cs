using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using DripChip.Domain.Entities;
using FluentValidation;
using Mapster;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.AnimalTypes.Commands;

public static class Update
{
    public sealed record Command(long Id, string Type) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id).AnimalTypeId();
            RuleFor(x => x.Type).NotEmpty();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context) => _context = context;

        public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var exists = await _context.AnimalTypes.AnyAsync(animalType => animalType.Id == request.Id, cancellationToken);
            if (!exists)
                throw new NotFoundException(nameof(AnimalType), request.Id);

            var sameTypeNameExists = await _context.AnimalTypes.AnyAsync(animalType => animalType.Type == request.Type, cancellationToken);
            if (sameTypeNameExists)
                throw new AlreadyExistsException();

            var entity = request.Adapt<AnimalType>();
            _context.AnimalTypes.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Adapt<Response>();
        }
    }

    public sealed record Response(long Id, string Type);
}