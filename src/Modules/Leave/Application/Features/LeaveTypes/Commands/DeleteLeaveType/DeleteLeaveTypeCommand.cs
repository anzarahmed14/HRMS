using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveTypes.Commands.DeleteLeaveType;

public record DeleteLeaveTypeCommand(
    Guid Id) : IRequest;
