using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDevices.Commands.UpdateAttendanceDevice;

public sealed class UpdateAttendanceDeviceCommandValidator
    : AbstractValidator<UpdateAttendanceDeviceCommand>
{
    public UpdateAttendanceDeviceCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Attendance device ID is required.");

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.SerialNumber)
            .MaximumLength(100);

        RuleFor(x => x.IpAddress)
            .MaximumLength(50);

        RuleFor(x => x.Location)
            .MaximumLength(200);
    }
}
