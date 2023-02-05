using FluentValidation.Results;

namespace DripChip.Application.Exceptions;

public class ValidationException : Exception
{
    public Dictionary<string, string[]> Errors { get; } = new();
    
    public ValidationException()
        : base("One or more validation failures have occurred.") { }

    public ValidationException(IEnumerable<ValidationFailure> failures) : this() => Errors = failures
        .GroupBy(
            failure => failure.PropertyName,
            failure => failure.ErrorMessage)
        .ToDictionary(
            group => group.Key,
            group => group.ToArray());
}