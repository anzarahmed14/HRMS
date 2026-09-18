namespace HRMS.Modules.Identity.Application.Authorization;

/// <summary>
/// Reconciles the code-side <see cref="PermissionCatalog"/> with the
/// <c>Permissions</c> table. Declared in the Application layer so command
/// handlers depend only on this abstraction; the database-backed
/// implementation lives in Identity.Infrastructure, keeping Application
/// free of any dependency on Infrastructure or EF Core.
/// </summary>
public interface IPermissionCatalogSynchronizer
{
    /// <summary>
    /// Creates any <see cref="PermissionCatalog"/> entry that has no
    /// matching <c>Permissions.Name</c> row yet. Idempotent — running it
    /// again with no catalog changes performs no writes. Never modifies,
    /// deactivates, or deletes an existing permission, and never touches
    /// <c>RolePermission</c> or <c>UserRole</c> assignments.
    /// </summary>
    Task<PermissionSynchronizationResult> SynchronizeAsync(
        CancellationToken cancellationToken = default);
}
