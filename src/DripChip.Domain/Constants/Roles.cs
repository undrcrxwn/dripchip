namespace DripChip.Domain.Constants;

public static class Roles
{
    public const string User = "USER";
    public const string Chipper = "CHIPPER";
    public const string Admin = "ADMIN";

    public static bool Contain(string role) => role is User or Chipper or Admin;
}