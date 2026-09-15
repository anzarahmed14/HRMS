using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Commands.BulkDeleteEmployeeLeaveEntitlements;

public class BulkDeleteEmployeeLeaveEntitlementsCommandHandler
    : IRequestHandler<
        BulkDeleteEmployeeLeaveEntitlementsCommand,
        BulkDeleteEmployeeLeaveEntitlementsResult>
{
    private readonly IReadRepository<EmployeeLeaveEntitlement, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeLeaveEntitlement, Guid> _writeRepository;
    private readonly IUnitOfWorkTransaction _transaction;

    public BulkDeleteEmployeeLeaveEntitlementsCommandHandler(
        IReadRepository<EmployeeLeaveEntitlement, Guid> readRepository,
        IWriteRepository<EmployeeLeaveEntitlement, Guid> writeRepository,
        IUnitOfWorkTransaction transaction)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _transaction = transaction;
    }

    public async Task<BulkDeleteEmployeeLeaveEntitlementsResult> Handle(
        BulkDeleteEmployeeLeaveEntitlementsCommand request,
        CancellationToken cancellationToken)
    {
        var entitlements = await _readRepository.FindAsync(
            x =>
                x.EmployeeId == request.EmployeeId &&
                x.LeaveYearId == request.LeaveYearId &&
                request.LeaveTypeIds.Contains(x.LeaveTypeId) &&
                !x.IsDeleted,
            cancellationToken);

        var entitlementList = entitlements.ToList();

        if (entitlementList.Count == 0)
        {
            return new BulkDeleteEmployeeLeaveEntitlementsResult
            {
                DeletedCount = 0
            };
        }

        await _transaction.BeginAsync(cancellationToken);

        try
        {
            await _writeRepository.DeleteRangeAsync(
                entitlementList,
                cancellationToken);

            await _transaction.CommitAsync(cancellationToken);

            return new BulkDeleteEmployeeLeaveEntitlementsResult
            {
                DeletedCount = entitlementList.Count
            };
        }
        catch
        {
            await _transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
