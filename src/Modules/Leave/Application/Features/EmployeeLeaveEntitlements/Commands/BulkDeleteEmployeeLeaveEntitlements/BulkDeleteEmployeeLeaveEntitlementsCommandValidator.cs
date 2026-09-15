using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Commands.BulkDeleteEmployeeLeaveEntitlements;

public class BulkDeleteEmployeeLeaveEntitlementsCommandValidator
    : AbstractValidator<BulkDeleteEmployeeLeaveEntitlementsCommand>
{
    public BulkDeleteEmployeeLeaveEntitlementsCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("EmployeeId is required.");

        RuleFor(x => x.LeaveYearId)
            .NotEmpty()
            .WithMessage("LeaveYearId is required.");

        RuleFor(x => x.LeaveTypeIds)
            .NotEmpty()
            .WithMessage("At least one LeaveTypeId is required.");

        RuleFor(x => x.LeaveTypeIds)
            .Must(HaveUniqueLeaveTypes)
            .WithMessage("Duplicate LeaveTypeId is not allowed.");
    }

    private static bool HaveUniqueLeaveTypes(List<Guid> leaveTypeIds)
    {
        return leaveTypeIds
            .Distinct()
            .Count() == leaveTypeIds.Count;
    }
}
