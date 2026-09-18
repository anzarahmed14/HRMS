namespace HRMS.Modules.Identity.Application.Authorization;

/// <summary>
/// The complete, centralized list of permissions declared by
/// <see cref="PermissionNames"/>, each paired with the <c>Modules.Code</c>
/// that owns it. This is the single input the permission catalog
/// synchronizer reads to reconcile the database — every entry here must
/// reference a <see cref="PermissionNames"/> constant rather than a literal
/// string, so the permission code is never duplicated.
/// </summary>
/// <remarks>
/// Adding a new permission to <see cref="PermissionNames"/> without adding
/// a corresponding entry here means it will never be created in the
/// database by synchronization — the two are intentionally kept as
/// separate, explicit steps rather than merged, so a developer must
/// deliberately decide which module owns the new permission.
/// </remarks>
public static class PermissionCatalog
{
    private const string IdentityModule = "IDENTITY";
    private const string EmployeeModule = "EMPLOYEE";
    private const string DepartmentModule = "DEPARTMENT";

    public static IReadOnlyList<PermissionCatalogEntry> Entries { get; } =
        new List<PermissionCatalogEntry>
        {
            new(PermissionNames.User.ResetPassword, IdentityModule),

            new(PermissionNames.Employee.View, EmployeeModule),
            new(PermissionNames.Employee.Create, EmployeeModule),
            new(PermissionNames.Employee.Update, EmployeeModule),
            new(PermissionNames.Employee.Delete, EmployeeModule),

            new(PermissionNames.Department.View, DepartmentModule),
            new(PermissionNames.Department.Create, DepartmentModule),
            new(PermissionNames.Department.Update, DepartmentModule),
            new(PermissionNames.Department.Delete, DepartmentModule),
        };
}
