using FluentValidation.Results;

namespace DripChip.Application.Exceptions;

public class ValidationException : Exception
{
    public IDictionary<string, IEnumerable<string>> Errors { get; } = new Dictionary<string, IEnumerable<string>>();

    public ValidationException()
        : base("One or more validation failures have occurred.") { }

    public ValidationException(IDictionary<string, IEnumerable<string>> errors) =>
        Errors = errors;

    public ValidationException(IEnumerable<string> errorDescriptions, string propertyName)
        : this(new Dictionary<string, IEnumerable<string>>
        {
            [propertyName] = errorDescriptions
        }) { }

    public ValidationException(IEnumerable<ValidationFailure> failures) : this(failures
        .GroupBy(
            failure => failure.PropertyName,
            failure => failure.ErrorMessage)
        .ToDictionary(
            failuresByPropertyName => failuresByPropertyName.Key,
            failuresByPropertyName => failuresByPropertyName.AsEnumerable())) { }
}