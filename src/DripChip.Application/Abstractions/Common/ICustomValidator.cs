using FluentValidation;

namespace DripChip.Application.Abstractions.Common;

public interface ICustomValidator<T, in TProperty>
{
    public Task ValidateAsync(ValidationContext<T> context, TProperty value);
}