using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Infrastructure.Persistence;

public static class NpgsqlMethodReferences
{
    /// <summary>
    /// An implementation of the PostgreSQL ILIKE operation, which is an insensitive LIKE.
    /// </summary>
    public static readonly MethodInfo CaseInsensitiveLikeMethod =
        typeof(NpgsqlDbFunctionsExtensions)
            .GetMethod(nameof(NpgsqlDbFunctionsExtensions.ILike),
                new[] { typeof(DbFunctions), typeof(string), typeof(string) })!;
}