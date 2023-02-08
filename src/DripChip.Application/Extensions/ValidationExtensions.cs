using DripChip.Application.Abstractions.Common;
using FluentValidation;

namespace DripChip.Application.Extensions;

public static class ValidationExtensions
{
    public static IRuleBuilder<T, TProperty> Apply<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        ICustomValidator<T, TProperty> customValidator) =>
        ruleBuilder.CustomAsync(async (property, context, _) =>
            await customValidator.ValidateAsync(context, property));
}