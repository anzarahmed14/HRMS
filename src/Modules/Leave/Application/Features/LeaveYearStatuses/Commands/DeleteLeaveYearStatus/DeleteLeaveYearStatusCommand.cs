using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveYearStatuses.Commands.DeleteLeaveYearStatus;

public record DeleteLeaveYearStatusCommand(
    Guid Id) : IRequest;
