using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
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
        string Email,
        string Password,
        string FirstName,
        string LastName) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator(IPasswordValidator<Command> passwordValidator)
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.Password).Apply(passwordValidator).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserService _users;

        public Handler(IApplicationDbContext context, IUserService users)
        {
            _context = context;
            _users = users;
        }

        public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var sameExists = await _users.Users.AnyAsync(user =>
                user.Email == request.Email, cancellationToken);

            if (sameExists)
                throw new AlreadyExistsException("User with the specified email already exists.");

            // User creation
            var errorDescriptions = await _users.CreateAsync(request.Email, request.Password);
            if (errorDescriptions is not null)
                throw new ValidationException(nameof(request.Password), errorDescriptions);

            var user =
                await _users.FindByEmailAsync(request.Email)
                ?? throw new InvalidOperationException();

            // Account creation
            var account = request.Adapt<Account>();
            account.Id = user.Id;
            await _context.Accounts.AddAsync(account, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            return new Response(user.Id, account.FirstName, account.LastName, user.Email!);
        }
    }

    public sealed record Response(int Id, string FirstName, string LastName, string Email);
}