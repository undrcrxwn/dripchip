using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;

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
        private readonly IUserRepository _users;
        private readonly ICurrentUserProvider _issuer;

        public Handler(IApplicationDbContext context, IUserRepository users, ICurrentUserProvider issuer)
        {
            _context = context;
            _users = users;
            _issuer = issuer;
        }

        public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.Id != _issuer.AccountId)
                throw new ForbiddenException();

            var query =
                from account in _context.Accounts
                join user in _users.Users on account.Id equals user.Id
                where user.Id == request.Id
                select new { Account = account, User = user };

            var result =
                await query.FirstOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException();

            result.Account.FirstName = request.FirstName;
            result.Account.LastName = request.LastName;

            await _users.SetEmailAsync(result.User, request.Email);
            await _users.SetUsernameAsync(result.User, request.Email);
            await _users.SetPasswordAsync(result.User, request.Password);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response(
                result.User.Id,
                result.Account.FirstName,
                result.Account.LastName,
                result.User.Email!);
        }
    }

    public sealed record Response(int Id, string FirstName, string LastName, string Email);
}