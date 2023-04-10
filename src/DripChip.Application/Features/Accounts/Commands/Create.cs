using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using DripChip.Application.Models.Identity;
using DripChip.Domain.Constants;
using DripChip.Domain.Entities;
using FluentValidation;
using Mapster;
using Mediator;
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
        public Validator(IPasswordValidator<Command> passwordValidator)
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
        private readonly IUserRepository _users;

        public Handler(ICurrentUserProvider issuer, IApplicationDbContext context, IUserRepository users)
        {
            _issuer = issuer;
            _context = context;
            _users = users;
        }

        public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var issuer = await _issuer.GetUserAsync();
            if (issuer?.Role != Roles.Admin && !_issuer.BypassAuthentication)
                throw new ForbiddenException();

            var sameExists = await _users.Users.AnyAsync(user =>
                user.Id == request.Id || user.Email == request.Email, cancellationToken);

            if (sameExists)
                throw new AlreadyExistsException("User with the specified identity already exists.");

            // User creation
            var userCreationResult = await _users.CreateAsync(request.Email, request.Password, request.Role, request.Id);
            if (userCreationResult is UserCreationResult.Failure failure)
                throw new ValidationException(nameof(request.Password), failure.Reasons);

            if (userCreationResult is not UserCreationResult.Success success)
                throw new InvalidOperationException();

            // Account creation
            var account = request.Adapt<Account>();
            if (request.Id is not null)
                account.Id = request.Id.Value;
            await _context.Accounts.AddAsync(account, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            return new Response(success.User.Id, account.FirstName, account.LastName, success.User.Email!, success.User.Role);
        }
    }

    public sealed record Response(int Id, string FirstName, string LastName, string Email, string Role);
}