using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Commands.BulkUpsertEmployeeLeaveEntitlements;

public class BulkUpsertEmployeeLeaveEntitlementsCommandValidator
    : AbstractValidator<BulkUpsertEmployeeLeaveEntitlementsCommand>
{
    public BulkUpsertEmployeeLeaveEntitlementsCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("EmployeeId is required.");

        RuleFor(x => x.LeaveYearId)
            .NotEmpty()
            .WithMessage("LeaveYearId is required.");

        RuleFor(x => x.Entitlements)
            .NotEmpty()
            .WithMessage("At least one leave entitlement is required.");

        RuleForEach(x => x.Entitlements)
            .SetValidator(new BulkEmployeeLeaveEntitlementItemValidator());

        RuleFor(x => x.Entitlements)
            .Must(HaveUniqueLeaveTypes)
            .WithMessage("Duplicate LeaveTypeId is not allowed in the same request.");
    }

    private static bool HaveUniqueLeaveTypes(
        List<BulkEmployeeLeaveEntitlementItem> entitlements)
    {
        return entitlements
            .Select(x => x.LeaveTypeId)
            .Distinct()
            .Count() == entitlements.Count;
    }
}

public class BulkEmployeeLeaveEntitlementItemValidator
    : AbstractValidator<BulkEmployeeLeaveEntitlementItem>
{
    public BulkEmployeeLeaveEntitlementItemValidator()
    {
        RuleFor(x => x.LeaveTypeId)
            .NotEmpty()
            .WithMessage("LeaveTypeId is required.");

        RuleFor(x => x.LeavePolicyRuleId)
            .NotEmpty()
            .WithMessage("LeavePolicyRuleId is required.");

        RuleFor(x => x.EntitledDays)
            .GreaterThanOrEqualTo(0)
            .WithMessage("EntitledDays cannot be negative.");
    }
}
