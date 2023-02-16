using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using FluentValidation;
using Mediator;

namespace DripChip.Application.Features.Accounts.Commands;

public static class Update
{
    public sealed record Command(
        int Id,
        string FirstName,
        string LastName,
        string Email,
        string Password) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator(IPasswordValidator<Command> passwordValidator)
        {
            RuleFor(x => x.Id).AccountId();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.Password).Apply(passwordValidator).NotEmpty();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserService _users;
        private readonly ICurrentUserProvider _issuer;

        public Handler(IApplicationDbContext context, IUserService users, ICurrentUserProvider issuer)
        {
            _context = context;
            _users = users;
            _issuer = issuer;
        }

        public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.Id != _issuer.AccountId)
                throw new ForbiddenException();

            var user =
                await _users.FindByIdAsync(request.Id)
                ?? throw new NotFoundException();

            var account =
                await _context.Accounts.FindAsync(request.Id)
                ?? throw new NotFoundException();

            account.FirstName = request.FirstName;
            account.LastName = request.LastName;

            await _users.SetEmailAsync(user, request.Email);
            await _users.SetUsernameAsync(user, request.Email);
            await _users.SetPasswordAsync(user, request.Password);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response(
                user.Id,
                account.FirstName,
                account.LastName,
                user.Email!);
        }
    }

    public sealed record Response(int Id, string FirstName, string LastName, string Email);
}