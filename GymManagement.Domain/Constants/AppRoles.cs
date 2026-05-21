namespace GymManagement.Domain.Constants;

public static class AppRoles
{
    public const string Admin = "Admin";
    public const string Manager = "Manager";
    public const string Receptionist = "Receptionist";
    public const string Trainer = "Trainer";
    public const string Member = "Member";

    public static readonly string[] All =
    {
        Admin,
        Manager,
        Receptionist,
        Trainer,
        Member
    };
}