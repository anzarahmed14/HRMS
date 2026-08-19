using MediatR;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Commands.DeleteEmployeeLeaveEntitlement;

public record DeleteEmployeeLeaveEntitlementCommand(
    Guid Id) : IRequest;
