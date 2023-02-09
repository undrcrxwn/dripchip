using DripChip.Application.Abstractions;
using FluentValidation;

namespace DripChip.Application.Extensions;

public static class ValidationExtensions
{
    public static IRuleBuilder<T, TProperty> Apply<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        ICustomValidator<T, TProperty> customValidator) =>
        ruleBuilder.CustomAsync(async (property, context, _) =>
            await customValidator.ValidateAsync(context, property));

    public static IRuleBuilder<T, int> AccountId<T>(
        this IRuleBuilder<T, int> ruleBuilder) =>
        ruleBuilder.GreaterThan(0);

    public static IRuleBuilder<T, long> LocationPointId<T>(
        this IRuleBuilder<T, long> ruleBuilder) =>
        ruleBuilder.GreaterThan(0);
    
    public static IRuleBuilder<T, long> AnimalTypeId<T>(
        this IRuleBuilder<T, long> ruleBuilder) =>
        ruleBuilder.GreaterThan(0);

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
}