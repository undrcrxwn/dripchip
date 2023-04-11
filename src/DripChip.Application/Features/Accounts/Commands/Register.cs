using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using DripChip.Domain.Constants;
using DripChip.Domain.Entities;
using FluentValidation;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.Accounts.Commands;

public static class Register
{
    public sealed record Command(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string? Role = Roles.User) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator(Abstractions.Identity.IPasswordValidator<Command> passwordValidator)
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.Password).Apply(passwordValidator).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Role!).Role();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<Account> _users;
        private readonly ICurrentUserProvider _issuer;

        public Handler(IApplicationDbContext context, UserManager<Account> users, ICurrentUserProvider issuer)
        {
            _context = context;
            _users = users;
            _issuer = issuer;
        }

        public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            if (_issuer.IsAuthenticated)
                throw new ForbiddenException();

            var sameExists = await _context.Accounts.AnyAsync(
                account => account.NormalizedEmail == _users.NormalizeEmail(request.Email),
                cancellationToken);

            if (sameExists)
                throw new AlreadyExistsException("User with the specified email already exists.");

            var account = request.Adapt<Account>();
            account.UserName = request.Email;
            await _users.CreateAsync(account, request.Password);

            return account.Adapt<Response>();
        }
    }

    public sealed record Response(int Id, string FirstName, string LastName, string Email, string Role);
}