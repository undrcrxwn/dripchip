using FluentValidation;

namespace DripChip.Application.Abstractions;

/// <summary>
/// Ensures that a given model's property is valid.
/// </summary>
/// <typeparam name="TModel">Validated property's parent model.</typeparam>
/// <typeparam name="TProperty">Property to be validated.</typeparam>
public interface ICustomValidator<TModel, in TProperty>
{
    /// <summary>
    /// Supplements the specified validation context with the given property's validation errors (if such are distinguished).
    /// </summary>
    /// <param name="context">Validation context to be modified. Validation errors destination.</param>
    /// <param name="value">Model's property to be validated.</param>
    public Task ValidateAsync(ValidationContext<TModel> context, TProperty value);
}