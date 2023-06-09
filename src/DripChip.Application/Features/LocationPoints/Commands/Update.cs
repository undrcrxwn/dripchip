using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using DripChip.Domain.Entities;
using FluentValidation;
using Mapster;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.LocationPoints.Commands;

public static class Update
{
    public sealed record Command(long Id, double Latitude, double Longitude) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id).LocationPointId();
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
            var exists = await _context.LocationPoints.AnyAsync(locationPoint => locationPoint.Id == request.Id, cancellationToken);
            if (!exists)
                throw new NotFoundException(nameof(LocationPoint), request.Id);

            var sameExists = await _context.LocationPoints.AnyAsync(x =>
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                x.Latitude == request.Latitude &&
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                x.Longitude == request.Longitude, cancellationToken);

            if (sameExists)
                throw new AlreadyExistsException();

            var entity = request.Adapt<LocationPoint>();
            _context.LocationPoints.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Adapt<Response>();
        }
    }

    public sealed record Response(long Id, double Latitude, double Longitude);
}