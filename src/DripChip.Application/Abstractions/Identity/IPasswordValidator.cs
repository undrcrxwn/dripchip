using DripChip.Application.Abstractions.Common;

namespace DripChip.Application.Abstractions.Identity;

public interface IPasswordValidator<T> : ICustomValidator<T, string> { }