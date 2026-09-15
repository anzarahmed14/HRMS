using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveDayParts.Commands.DeleteLeaveDayPart;

public record DeleteLeaveDayPartCommand(
    Guid Id) : IRequest;
