using DripChip.Domain.Entities;
using DripChip.Infrastructure.Identity.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using ValidationException = DripChip.Application.Exceptions.ValidationException;

namespace DripChip.Application.Services;

/// <summary>
/// Tells if a string is a valid user password that satisfies ASP.NET Core Identity requirements.
/// </summary>
public class PasswordValidator<T> : Application.Abstractions.Identity.IPasswordValidator<T>
{
    private readonly UserManager<Account> _userManager;

    public PasswordValidator(UserManager<Account> userManager) =>
        _userManager = userManager;

    public async Task ValidateAsync(ValidationContext<T> context, string value)
    {
        var validationTasks = _userManager.PasswordValidators
            .Select(validator => validator.ValidateAsync(_userManager, null!, value))
            .ToArray();

        var validationResults = await Task.WhenAll(validationTasks);
        var validationErrors = validationResults
            .SelectMany(result => result.Errors)
            .ToArray();

        if (validationErrors.Any())
            throw new ValidationException(validationErrors.ToValidationFailures(context.PropertyName));
    }
}