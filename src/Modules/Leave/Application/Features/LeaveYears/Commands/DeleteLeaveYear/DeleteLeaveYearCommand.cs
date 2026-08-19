using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveYears.Commands.DeleteLeaveYear;

public record DeleteLeaveYearCommand(
    Guid Id) : IRequest;
