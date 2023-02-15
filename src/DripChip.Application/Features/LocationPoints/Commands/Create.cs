using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using DripChip.Domain.Entities;
using FluentValidation;
using Mapster;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.LocationPoints.Commands;

public static class Create
{
    public sealed record Command(double Latitude, double Longitude) : IRequest<Response>;

    private sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Latitude).Latitude();
            RuleFor(x => x.Longitude).Longitude();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context) => _context = context;

        public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var sameExists = await _context.LocationPoints.AnyAsync(x =>
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                x.Latitude == request.Latitude &&
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                x.Longitude == request.Longitude);

            if (sameExists)
                throw new AlreadyExistsException();
        
            var entity = request.Adapt<LocationPoint>();
            await _context.LocationPoints.AddAsync(entity);
            await _context.SaveChangesAsync(cancellationToken);
        
            return entity.Adapt<Response>();
        }
    }

    public sealed record Response(long Id, double Latitude, double Longitude);
}