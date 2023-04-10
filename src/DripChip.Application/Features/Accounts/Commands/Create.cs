using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using DripChip.Application.Models.Identity;
using DripChip.Domain.Constants;
using DripChip.Domain.Entities;
using DripChip.Infrastructure.Identity.Extensions;
using FluentValidation;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ValidationException = DripChip.Application.Exceptions.ValidationException;

namespace DripChip.Application.Features.Accounts.Commands;

public static class Create
{
    public sealed record Command(
        int? Id,
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string Role) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator(Abstractions.Identity.IPasswordValidator<Command> passwordValidator)
        {
            When(x => x.Id is not null, () => RuleFor(x => x.Id!.Value).AccountId());
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.Password).Apply(passwordValidator).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Role).NotEmpty();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly ICurrentUserProvider _issuer;
        private readonly IApplicationDbContext _context;
        private readonly UserManager<Account> _users;

        public Handler(ICurrentUserProvider issuer, IApplicationDbContext context, UserManager<Account> users)
        {
            _issuer = issuer;
            _context = context;
            _users = users;
        }

        public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var issuer = await _issuer.GetAccountAsync();
            if (issuer?.Role != Roles.Admin && !_issuer.BypassAuthentication)
                throw new ForbiddenException();

            var sameExists = await _context.Accounts.AnyAsync(user =>
                user.Id == request.Id || user.Email == request.Email, cancellationToken);

            if (sameExists)
                throw new AlreadyExistsException("User with the specified identity already exists.");

            var account = request.Adapt<Account>();
            account.UserName = request.Email;
            await _users.CreateAsync(account, request.Password);

            return account.Adapt<Response>();
        }
    }

    public sealed record Response(int Id, string FirstName, string LastName, string Email, string Role);
}