using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDevices.Commands.CreateAttendanceDevice;

public sealed class CreateAttendanceDeviceCommandValidator
    : AbstractValidator<CreateAttendanceDeviceCommand>
{
    public CreateAttendanceDeviceCommandValidator()
    {
        RuleFor(x => x.AttendanceSourceId)
            .NotEmpty()
            .WithMessage("Attendance source is required.");

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
