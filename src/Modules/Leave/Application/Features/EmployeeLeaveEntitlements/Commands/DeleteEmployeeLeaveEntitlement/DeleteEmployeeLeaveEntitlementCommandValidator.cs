using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Commands.DeleteEmployeeLeaveEntitlement;

public class DeleteEmployeeLeaveEntitlementCommandValidator
    : AbstractValidator<DeleteEmployeeLeaveEntitlementCommand>
{
    public DeleteEmployeeLeaveEntitlementCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
