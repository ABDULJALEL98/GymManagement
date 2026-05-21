namespace GymManagement.Domain.Constants;

public static class Permissions
{
    public static class SubscriptionPlans
    {
        public const string View = "Permissions.SubscriptionPlans.View";
        public const string Create = "Permissions.SubscriptionPlans.Create";
        public const string Update = "Permissions.SubscriptionPlans.Update";
        public const string Delete = "Permissions.SubscriptionPlans.Delete";
    }

    public static class Members
    {
        public const string View = "Permissions.Members.View";
        public const string Create = "Permissions.Members.Create";
        public const string Update = "Permissions.Members.Update";
        public const string Delete = "Permissions.Members.Delete";
    }

    public static class Trainers
    {
        public const string View = "Permissions.Trainers.View";
        public const string Create = "Permissions.Trainers.Create";
        public const string Update = "Permissions.Trainers.Update";
        public const string Delete = "Permissions.Trainers.Delete";
    }

    public static class Bookings
    {
        public const string View = "Permissions.Bookings.View";
        public const string Create = "Permissions.Bookings.Create";
        public const string Update = "Permissions.Bookings.Update";
        public const string Delete = "Permissions.Bookings.Delete";
    }

    public static class Payments
    {
        public const string View = "Permissions.Payments.View";
        public const string Create = "Permissions.Payments.Create";
        public const string Update = "Permissions.Payments.Update";
        public const string Delete = "Permissions.Payments.Delete";
    }

    public static class Roles
    {
        public const string View = "Permissions.Roles.View";
        public const string Manage = "Permissions.Roles.Manage";
    }

    public static class Users
    {
        public const string View = "Permissions.Users.View";
        public const string Create = "Permissions.Users.Create";
        public const string Update = "Permissions.Users.Update";
        public const string Delete = "Permissions.Users.Delete";
    }

    public static List<string> GetAllPermissions()
    {
        return typeof(Permissions)
            .GetNestedTypes()
            .SelectMany(type => type.GetFields())
            .Select(field => field.GetValue(null)?.ToString())
            .Where(permission => !string.IsNullOrWhiteSpace(permission))
            .Select(permission => permission!)
            .ToList();
    }
}