using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using DripChip.Domain.Constants;
using FluentValidation;
using Mediator;

namespace DripChip.Application.Features.Areas.Commands;

public static class Delete
{
    public sealed record Command(long Id) : IRequest;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator() => RuleFor(x => x.Id).AreaId();
    }

    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserProvider _issuer;

        public Handler(IApplicationDbContext context, ICurrentUserProvider issuer)
        {
            _context = context;
            _issuer = issuer;
        }

        public async ValueTask<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var issuer = await _issuer.GetAccountAsync();
            if (issuer?.Role != Roles.Admin)
                throw new ForbiddenException();
            
            var area =
                await _context.Areas.FindAsync(request.Id)
                ?? throw new NotFoundException();

            _context.AreaPoints.RemoveRange(area.AreaPoints);
            _context.Areas.Remove(area);

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}