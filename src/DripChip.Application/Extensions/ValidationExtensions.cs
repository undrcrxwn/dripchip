using DripChip.Application.Abstractions;
using DripChip.Domain.Constants;
using FluentValidation;

namespace DripChip.Application.Extensions;

public static class ValidationExtensions
{
    public static IRuleBuilder<T, TProperty> Apply<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        ICustomValidator<T, TProperty> customValidator) =>
        ruleBuilder.CustomAsync(async (property, context, _) =>
            await customValidator.ValidateAsync(context, property));

    /// <summary>
    /// Checks whether the given property can be parsed into a value of the specified enum.
    /// </summary>
    public static IRuleBuilder<T, string> IsInEnum<T>(
        this IRuleBuilder<T, string> ruleBuilder, Type enumType, bool ignoreCase = true) =>
        ruleBuilder.Custom((property, context) =>
        {
            if (!Enum.TryParse(enumType, property, ignoreCase, out _))
                context.AddFailure(context.PropertyName,
                    $"'{property}' is not a valid value for '{enumType.Name}'.");
        });

    public static IRuleBuilder<T, double> Latitude<T>(
        this IRuleBuilder<T, double> ruleBuilder) =>
        ruleBuilder
            .GreaterThanOrEqualTo(-90)
            .LessThanOrEqualTo(90);

    public static IRuleBuilder<T, double> Longitude<T>(
        this IRuleBuilder<T, double> ruleBuilder) =>
        ruleBuilder
            .GreaterThanOrEqualTo(-180)
            .LessThanOrEqualTo(180);

    public static IRuleBuilder<T, long> LocationPointId<T>(
        this IRuleBuilder<T, long> ruleBuilder) =>
        ruleBuilder.GreaterThan(0);

    public static IRuleBuilder<T, int> AccountId<T>(
        this IRuleBuilder<T, int> ruleBuilder) =>
        ruleBuilder.GreaterThan(0);

    public static IRuleBuilder<T, long> AnimalId<T>(
        this IRuleBuilder<T, long> ruleBuilder) =>
        ruleBuilder.GreaterThan(0);

    public static IRuleBuilder<T, long> AnimalTypeId<T>(
        this IRuleBuilder<T, long> ruleBuilder) =>
        ruleBuilder.GreaterThan(0);

    public static IRuleBuilder<T, long> AnimalLocationVisitId<T>(
        this IRuleBuilder<T, long> ruleBuilder) =>
        ruleBuilder.GreaterThan(0);
    
    public static IRuleBuilder<T, long> AreaId<T>(
        this IRuleBuilder<T, long> ruleBuilder) =>
        ruleBuilder.GreaterThan(0);

    public static IRuleBuilder<T, string> Role<T>(
        this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder.Custom((property, context) =>
        {
            if (!Roles.Contain(property))
                context.AddFailure(context.PropertyName, $"'{property}' is not a valid role.");
        });
}