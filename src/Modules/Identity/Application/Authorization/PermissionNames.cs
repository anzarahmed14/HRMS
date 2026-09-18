namespace HRMS.Modules.Identity.Application.Authorization;

/// <summary>
/// Centralized, developer-facing catalog of permission codes used across
/// the application. Permission codes must be referenced through this
/// class rather than duplicated as magic strings.
/// </summary>
public static class PermissionNames
{
    public static class User
    {
        public const string ResetPassword = "User.ResetPassword";
    }

    public static class Employee
    {
        public const string View = "Employee.View";
        public const string Create = "Employee.Create";
        public const string Update = "Employee.Update";
        public const string Delete = "Employee.Delete";
    }

    public static class Department
    {
        public const string View = "Department.View";
        public const string Create = "Department.Create";
        public const string Update = "Department.Update";
        public const string Delete = "Department.Delete";
    }
}
