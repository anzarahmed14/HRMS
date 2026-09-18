using HRMS.Modules.Identity.Application.Authorization;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.PermissionCatalog.Commands.SynchronizePermissionCatalog;

public class SynchronizePermissionCatalogCommandHandler
    : IRequestHandler<SynchronizePermissionCatalogCommand, PermissionSynchronizationResult>
{
    private readonly IPermissionCatalogSynchronizer _synchronizer;

    public SynchronizePermissionCatalogCommandHandler(
        IPermissionCatalogSynchronizer synchronizer)
    {
        _synchronizer = synchronizer;
    }

    public Task<PermissionSynchronizationResult> Handle(
        SynchronizePermissionCatalogCommand request,
        CancellationToken cancellationToken)
        => _synchronizer.SynchronizeAsync(cancellationToken);
}
