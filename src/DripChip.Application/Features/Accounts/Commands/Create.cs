using DripChip.Application.Abstractions;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using DripChip.Domain.Constants;
using DripChip.Domain.Entities;
using FluentValidation;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Identity;

namespace DripChip.Application.Features.Accounts.Commands;

public static class Create
{
    public sealed record Command(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string Role,
        int? Id = default) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator(Abstractions.Identity.IPasswordValidator<Command> passwordValidator)
        {
            When(x => x.Id.HasValue, () => RuleFor(x => x.Id!.Value).AccountId());
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
        private readonly UserManager<Account> _users;

        public Handler(ICurrentUserProvider issuer, UserManager<Account> users)
        {
            _issuer = issuer;
            _users = users;
        }

        public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var issuer = await _issuer.GetAccountAsync();
            if (issuer?.Role != Roles.Admin)
                throw new ForbiddenException();

            if (request.Id is { } accountId)
            {
                var sameExists =
                    (await _users.FindByIdAsync(accountId.ToString()) ?? await _users.FindByEmailAsync(request.Email))
                    is not null;

                if (sameExists)
                    throw new AlreadyExistsException();
            }

            var account = request.Adapt<Account>();
            account.UserName = request.Email;
            await _users.CreateAsync(account, request.Password);

            return account.Adapt<Response>();
        }
    }

    public sealed record Response(int Id, string FirstName, string LastName, string Email, string Role);
}