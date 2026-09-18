namespace HRMS.Modules.Identity.Application.Authorization;

/// <summary>
/// One declared permission from <see cref="PermissionNames"/>, together with
/// the <see cref="Module"/> code that owns it. This is the unit of data the
/// permission catalog synchronizer reconciles against the database — it
/// never carries a database identifier, since the catalog is a code-side
/// declaration, not a persisted record.
/// </summary>
/// <param name="Name">
/// The permission code, always one of the <see cref="PermissionNames"/>
/// constants — never a duplicated literal.
/// </param>
/// <param name="ModuleCode">
/// The <c>Modules.Code</c> value (e.g. <c>"EMPLOYEE"</c>) that owns this
/// permission. This mapping is declared explicitly here rather than
/// inferred from the permission name's prefix, because the two do not
/// always match (for example <c>User.ResetPassword</c> belongs to the
/// <c>IDENTITY</c> module, not a non-existent <c>USER</c> module).
/// </param>
public sealed record PermissionCatalogEntry(
    string Name,
    string ModuleCode);
