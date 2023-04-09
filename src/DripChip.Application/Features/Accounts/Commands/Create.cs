using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using DripChip.Application.Models.Identity;
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
        int Id,
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string Role) : IRequest;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator(IPasswordValidator<Command> passwordValidator)
        {
            RuleFor(x => x.Id).AccountId();
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.Password).Apply(passwordValidator).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Role).NotEmpty();
        }
    }

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserRepository _users;

        public Handler(IApplicationDbContext context, IUserRepository users)
        {
            _context = context;
            _users = users;
        }

        public async ValueTask<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var sameExists = await _users.Users.AnyAsync(user =>
                user.Id == request.Id || user.Email == request.Email, cancellationToken);

            if (sameExists)
                throw new AlreadyExistsException("User with the specified identity already exists.");

            // User creation
            var userCreationResult = await _users.CreateAsync(request.Email, request.Password);
            if (userCreationResult is UserCreationResult.Failure failure)
                throw new ValidationException(nameof(request.Password), failure.Reasons);

            if (userCreationResult is not UserCreationResult.Success)
                throw new InvalidOperationException();

            // Account creation
            var account = request.Adapt<Account>();
            account.Id = request.Id;
            await _context.Accounts.AddAsync(account, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}