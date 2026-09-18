namespace HRMS.Modules.Identity.Application.Authorization;

/// <summary>
/// Outcome of one permission catalog synchronization run. Every list here
/// is informational — synchronization only ever creates the permissions
/// listed in <see cref="Created"/>; nothing else in the database is
/// modified, so <see cref="AlreadyPresent"/>, <see cref="Orphaned"/>, and
/// <see cref="Unresolved"/> exist purely for logging and manual review.
/// </summary>
public sealed class PermissionSynchronizationResult
{
    /// <summary>
    /// Permission names that did not exist in the database and were
    /// created by this run.
    /// </summary>
    public IReadOnlyList<string> Created { get; init; } = [];

    /// <summary>
    /// Permission names from the catalog that already existed in the
    /// database and were left completely untouched (no field of the
    /// existing row is ever modified by synchronization).
    /// </summary>
    public IReadOnlyList<string> AlreadyPresent { get; init; } = [];

    /// <summary>
    /// Permission names present in the database but not declared in
    /// <see cref="PermissionCatalog"/>. Never deleted, renamed, or
    /// deactivated automatically — reported so a human can decide whether
    /// they are legitimate (e.g. not yet migrated to the catalog) or stale.
    /// </summary>
    public IReadOnlyList<string> Orphaned { get; init; } = [];

    /// <summary>
    /// Catalog entries whose declared <c>ModuleCode</c> does not match any
    /// row in the <c>Modules</c> table. These are skipped — never created
    /// with a guessed or empty module — and reported so the mismatch can be
    /// fixed in <see cref="PermissionCatalog"/> or the <c>Modules</c> table.
    /// </summary>
    public IReadOnlyList<string> Unresolved { get; init; } = [];
}
