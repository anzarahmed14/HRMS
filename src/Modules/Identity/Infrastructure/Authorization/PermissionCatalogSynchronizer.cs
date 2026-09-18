using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Identity.Application.Authorization;
using HRMS.Modules.Identity.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace HRMS.Modules.Identity.Infrastructure.Authorization;

/// <summary>
/// Database-backed implementation of <see cref="IPermissionCatalogSynchronizer"/>.
/// Reconciles <see cref="PermissionCatalog"/> against the <c>Permissions</c>
/// table using the existing generic repositories — no raw SQL, no new
/// persistence abstraction, and no dependency on <c>ApplicationDbContext</c>
/// directly.
/// </summary>
/// <remarks>
/// Governance rules enforced here (see Phase 8 design):
/// <list type="bullet">
/// <item>Only ever <em>creates</em> missing permissions; an existing
/// <c>Permissions</c> row is never updated, deactivated, or deleted.</item>
/// <item>Never touches <c>RolePermission</c> or <c>UserRole</c> rows.</item>
/// <item>A catalog entry whose <c>ModuleCode</c> has no matching
/// <c>Modules.Code</c> row is skipped, not created with a guessed module.</item>
/// <item>All missing permissions are created in a single
/// <see cref="IWriteRepository{TEntity,TKey}.AddRangeAsync"/> call, which
/// EF Core executes as a single atomic <c>SaveChanges</c> — either every
/// missing permission is created, or none are.</item>
/// <item>Running this with no catalog changes performs zero writes
/// (idempotent).</item>
/// </list>
/// </remarks>
public class PermissionCatalogSynchronizer : IPermissionCatalogSynchronizer
{
    private readonly IReadRepository<Module, Guid> _moduleReadRepository;
    private readonly IReadRepository<Permission, Guid> _permissionReadRepository;
    private readonly IWriteRepository<Permission, Guid> _permissionWriteRepository;
    private readonly ILogger<PermissionCatalogSynchronizer> _logger;

    public PermissionCatalogSynchronizer(
        IReadRepository<Module, Guid> moduleReadRepository,
        IReadRepository<Permission, Guid> permissionReadRepository,
        IWriteRepository<Permission, Guid> permissionWriteRepository,
        ILogger<PermissionCatalogSynchronizer> logger)
    {
        _moduleReadRepository = moduleReadRepository;
        _permissionReadRepository = permissionReadRepository;
        _permissionWriteRepository = permissionWriteRepository;
        _logger = logger;
    }

    public async Task<PermissionSynchronizationResult> SynchronizeAsync(
        CancellationToken cancellationToken = default)
    {
        var modulesByCode = (await _moduleReadRepository.GetAllAsync(cancellationToken))
            .ToDictionary(x => x.Code, x => x.Id, StringComparer.OrdinalIgnoreCase);

        var existingPermissionNames = (await _permissionReadRepository.GetAllAsync(cancellationToken))
            .Select(x => x.Name)
            .ToHashSet(StringComparer.Ordinal);

        var alreadyPresent = new List<string>();
        var unresolved = new List<string>();
        var toCreate = new List<Permission>();
        var seenInCatalog = new HashSet<string>(StringComparer.Ordinal);

        foreach (var entry in PermissionCatalog.Entries)
        {
            if (!seenInCatalog.Add(entry.Name))
            {
                _logger.LogWarning(
                    "Duplicate permission catalog entry '{PermissionName}' was skipped.",
                    entry.Name);
                continue;
            }

            if (existingPermissionNames.Contains(entry.Name))
            {
                alreadyPresent.Add(entry.Name);
                continue;
            }

            if (!modulesByCode.TryGetValue(entry.ModuleCode, out var moduleId))
            {
                _logger.LogWarning(
                    "Permission '{PermissionName}' declares unknown module code '{ModuleCode}' and was not created.",
                    entry.Name,
                    entry.ModuleCode);
                unresolved.Add(entry.Name);
                continue;
            }

            toCreate.Add(new Permission
            {
                Id = Guid.NewGuid(),
                Name = entry.Name,
                ModuleId = moduleId,
                IsActive = true
            });
        }

        var orphaned = existingPermissionNames
            .Where(name => !seenInCatalog.Contains(name))
            .ToList();

        if (toCreate.Count > 0)
        {
            await _permissionWriteRepository.AddRangeAsync(toCreate, cancellationToken);

            _logger.LogInformation(
                "Permission catalog synchronization created {Count} permission(s): {Names}.",
                toCreate.Count,
                string.Join(", ", toCreate.Select(x => x.Name)));
        }
        else
        {
            _logger.LogInformation(
                "Permission catalog synchronization made no changes — all catalog permissions already exist.");
        }

        return new PermissionSynchronizationResult
        {
            Created = toCreate.Select(x => x.Name).ToList(),
            AlreadyPresent = alreadyPresent,
            Orphaned = orphaned,
            Unresolved = unresolved
        };
    }
}
