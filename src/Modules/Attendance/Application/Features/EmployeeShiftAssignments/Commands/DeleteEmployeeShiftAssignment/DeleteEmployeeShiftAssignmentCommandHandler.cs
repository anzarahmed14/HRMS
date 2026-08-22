using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.Commands.DeleteEmployeeShiftAssignment;

public sealed class DeleteEmployeeShiftAssignmentCommandHandler
    : IRequestHandler<DeleteEmployeeShiftAssignmentCommand>
{
    private readonly IReadRepository<EmployeeShiftAssignment, Guid>
        _readRepository;

    private readonly IWriteRepository<EmployeeShiftAssignment, Guid>
        _writeRepository;

    public DeleteEmployeeShiftAssignmentCommandHandler(
        IReadRepository<EmployeeShiftAssignment, Guid> readRepository,
        IWriteRepository<EmployeeShiftAssignment, Guid> writeRepository)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
    }

    public async Task Handle(
        DeleteEmployeeShiftAssignmentCommand request,
        CancellationToken cancellationToken)
    {
        var assignment = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (assignment is null || assignment.IsDeleted)
        {
            throw new NotFoundException(
                "Employee Shift Assignment",
                request.Id);
        }

        await _writeRepository.DeleteAsync(
            assignment,
            cancellationToken);
    }
}
