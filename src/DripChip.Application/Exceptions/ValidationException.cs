using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace DripChip.Application.Exceptions;

public class ValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();

    public ValidationException()
        : base("One or more validation failures have occurred.") { }

    public ValidationException(IDictionary<string, string[]> errors) => Errors = errors;
        
    public ValidationException(IEnumerable<ValidationFailure> failures) : this(failures
        .GroupBy(
            failure => failure.PropertyName,
            failure => failure.ErrorMessage)
        .ToDictionary(
            failuresByPropertyName => failuresByPropertyName.Key,
            failuresByPropertyName => failuresByPropertyName.ToArray())) { }

    public ValidationException(IEnumerable<IdentityError> errors, string propertyName) : this(errors
        .Select(error => new ValidationFailure(
            propertyName: propertyName,
            errorMessage: error.Description))
        .ToArray()) { }
}