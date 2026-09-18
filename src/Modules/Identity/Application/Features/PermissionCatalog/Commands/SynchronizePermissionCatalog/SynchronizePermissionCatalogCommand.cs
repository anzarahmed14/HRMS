using HRMS.Modules.Identity.Application.Authorization;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.PermissionCatalog.Commands.SynchronizePermissionCatalog;

/// <summary>
/// Explicitly triggers reconciliation of <see cref="Authorization.PermissionCatalog"/>
/// against the database. Deliberately not run automatically at startup —
/// it is an administrative action, invoked on demand.
/// </summary>
public record SynchronizePermissionCatalogCommand
    : IRequest<PermissionSynchronizationResult>;
