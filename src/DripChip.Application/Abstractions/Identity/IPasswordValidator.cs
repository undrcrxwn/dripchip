namespace DripChip.Application.Abstractions.Identity;

/// <summary>
/// Tells if a string is a valid user password.
/// </summary>
public interface IPasswordValidator<T> : ICustomValidator<T, string> { }