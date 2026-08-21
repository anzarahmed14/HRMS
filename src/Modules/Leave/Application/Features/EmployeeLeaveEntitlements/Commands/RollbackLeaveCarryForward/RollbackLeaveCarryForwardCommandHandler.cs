using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Commands.RollbackLeaveCarryForward;

public sealed class RollbackLeaveCarryForwardCommandHandler
    : IRequestHandler<RollbackLeaveCarryForwardCommand>
{
    private readonly IReadRepository<EmployeeLeaveEntitlement, Guid>
        _readRepository;

    private readonly IWriteRepository<EmployeeLeaveEntitlement, Guid>
        _writeRepository;

    private readonly IUnitOfWorkTransaction _transaction;

    public RollbackLeaveCarryForwardCommandHandler(
        IReadRepository<EmployeeLeaveEntitlement, Guid> readRepository,
        IWriteRepository<EmployeeLeaveEntitlement, Guid> writeRepository,
        IUnitOfWorkTransaction transaction)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _transaction = transaction;
    }

    public async Task Handle(
        RollbackLeaveCarryForwardCommand request,
        CancellationToken cancellationToken)
    {
        await _transaction.BeginAsync(cancellationToken);

        try
        {
            // Find carry-forward entitlements for the target year.
            var entitlements =
                await _readRepository.FindAsync(
                    x =>
                        x.LeaveYearId == request.LeaveYearId &&
                        x.CarryForwardDays > 0 &&
                        !x.IsDeleted,
                    cancellationToken);

            // Nothing to rollback.
            if (!entitlements.Any())
            {
                await _transaction.CommitAsync(cancellationToken);
                return;
            }

            // Never rollback a balance that has already been consumed.
            var usedEntitlement =
                entitlements.FirstOrDefault(
                    x => x.UsedDays > 0);

            if (usedEntitlement is not null)
            {
                throw new ConflictException(
                    "Carry forward cannot be rolled back because leave has already been used.");
            }

            // Soft delete generated carry-forward entitlements.
            foreach (var entitlement in entitlements)
            {
                entitlement.IsDeleted = true;
                entitlement.DeletedOn = DateTimeOffset.UtcNow;
            }

            await _writeRepository.UpdateRangeAsync(
                entitlements,
                cancellationToken);

            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await _transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
