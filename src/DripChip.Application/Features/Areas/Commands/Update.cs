using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using DripChip.Domain.Constants;
using DripChip.Domain.Entities;
using FluentValidation;
using Mapster;
using Mediator;

namespace DripChip.Application.Features.Areas.Commands;

public static class Update
{
    public sealed record AreaPointDto(double Longitude, double Latitude);

    public sealed record Command(long Id, string Name, IEnumerable<AreaPointDto> AreaPoints) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator() => RuleFor(x => x.Id).AreaId();
    }

    public sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserProvider _issuer;

        public Handler(IApplicationDbContext context, ICurrentUserProvider issuer)
        {
            _context = context;
            _issuer = issuer;
        }

        public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var issuer = await _issuer.GetAccountAsync();
            if (issuer?.Role != Roles.Admin)
                throw new ForbiddenException();

            var area =
                await _context.Areas.FindAsync(request.Id)
                ?? throw new NotFoundException();

            area.Name = request.Name;
            _context.AreaPoints.RemoveRange(area.AreaPoints);

            area.AreaPoints = request.AreaPoints.Select((point, i) => new AreaPoint
            {
                Longitude = point.Longitude,
                Latitude = point.Latitude,
                Area = area,
                SequenceId = i
            }).ToList();

            await _context.SaveChangesAsync(cancellationToken);
            return area.Adapt<Response>();
        }
    }

    public sealed record Response(long Id, string Name, IEnumerable<AreaPointDto> AreaPoints);
}