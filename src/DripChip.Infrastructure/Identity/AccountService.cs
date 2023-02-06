using System.ComponentModel.DataAnnotations;
using DripChip.Application.Abstractions.Common;
using DripChip.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using DripChip.Application.Extensions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Infrastructure.Identity;

public class AccountService : IAccountService
{
    private readonly IFilterFactory _filterFactory;
    private readonly UserManager<Account> _userManager;

    public AccountService(IFilterFactory filterFactory, UserManager<Account> userManager)
    {
        _filterFactory = filterFactory;
        _userManager = userManager;
    }

    public async Task<RegisterAccountResponse> RegisterAsync(RegisterAccountRequest request)
    {
        // Email uniqueness validation
        var account = await _userManager.FindByEmailAsync(request.Email);
        if (account is not null)
            throw new InvalidOperationException("Account with the specified email already exists.");

        account = new Account
        {
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email
        };

        // Password validation via user manager
        var passwordValidationTasks = _userManager.PasswordValidators
            .Select(x => x.ValidateAsync(_userManager, account, request.Password))
            .ToArray();

        // Validate data annotations
        var context = new ValidationContext(account);
        var validationErrors = new List<ValidationResult>();

        // Validate password
        var passwordValidationResults = await Task.WhenAll(passwordValidationTasks);
        foreach (var identityError in passwordValidationResults.SelectMany(x => x.Errors))
        {
            var validationError = new ValidationResult(identityError.Description, new[] { nameof(request.Password) });
            validationErrors.Add(validationError);
        }

        var isModelValid = Validator.TryValidateObject(account, context, validationErrors);
        if (!isModelValid)
            throw new InvalidOperationException(
                string.Join(Environment.NewLine, validationErrors.Select(x =>
                    $"{x.MemberNames.First()}: {x.ErrorMessage}")));

        // User creation
        var result = await _userManager.CreateAsync(account, request.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => x.Description);
            var description = string.Join(Environment.NewLine, errors);
            throw new InvalidOperationException($"User cannot be created.\n{description}");
        }

        return new RegisterAccountResponse
        {
            Id = account.Id,
            FirstName = account.FirstName,
            LastName = account.LastName,
            Email = account.Email
        };
    }

    public async Task<AccountResponse> GetByIdAsync(int accountId)
    {
        var account =
            await _userManager.FindByIdAsync(accountId.ToString())
            ?? throw new InvalidOperationException("Account not found.");

        return new AccountResponse
        {
            Id = account.Id,
            FirstName = account.FirstName,
            LastName = account.LastName,
            Email = account.Email!
        };
    }

    public async Task<IEnumerable<AccountResponse>> SearchAsync(SearchAccountRequest request)
    {
        var accounts = _userManager.Users
            // Filtering
            .Where(_filterFactory.CreateCaseInsensitiveContainsFilter<Account>(x => x.FirstName, request.FirstName))
            .Where(_filterFactory.CreateCaseInsensitiveContainsFilter<Account>(x => x.LastName, request.LastName))
            .Where(_filterFactory.CreateCaseInsensitiveContainsFilter<Account>(x => x.Email!, request.Email))
            // Pagination
            .Skip(request.From)
            .Take(request.Size);

        return await accounts
            .ProjectToType<AccountResponse>()
            .ToListAsync();
    }
}