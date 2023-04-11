using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using DripChip.Domain.Constants;
using DripChip.Domain.Entities;
using DripChip.Infrastructure.Identity.Extensions;
using FluentValidation;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Identity;

namespace DripChip.Application.Features.Accounts.Commands;

public static class Update
{
    public sealed record Command(
        int Id,
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string Role) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator(Abstractions.Identity.IPasswordValidator<Command> passwordValidator)
        {
            RuleFor(x => x.Id).AccountId();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.Password).Apply(passwordValidator).NotEmpty();
            RuleFor(x => x.Role).NotEmpty();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserProvider _issuer;
        private readonly UserManager<Account> _users;

        public Handler(IApplicationDbContext context, ICurrentUserProvider issuer, UserManager<Account> users)
        {
            _context = context;
            _issuer = issuer;
            _users = users;
        }

        public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var issuer = await _issuer.GetAccountAsync();
            if (issuer?.Id != request.Id && issuer?.Role != Roles.Admin)
                throw new ForbiddenException();

            var account =
                await _context.Accounts.FindAsync(request.Id)
                ?? throw new NotFoundException();

            account.FirstName = request.FirstName;
            account.LastName = request.LastName;
            account.Role = request.Role;

            await _users.SetEmailAsync(account, request.Email);
            await _users.SetUserNameAsync(account, request.Email);
            await _users.SetPasswordAsync(account, request.Password);
            await _context.SaveChangesAsync(cancellationToken);

            return account.Adapt<Response>();
        }
    }

    public sealed record Response(int Id, string FirstName, string LastName, string Email, string Role);
}