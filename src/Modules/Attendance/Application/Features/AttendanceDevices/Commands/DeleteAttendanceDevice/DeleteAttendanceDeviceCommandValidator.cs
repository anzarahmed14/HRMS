using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDevices.Commands.DeleteAttendanceDevice;

public sealed class DeleteAttendanceDeviceCommandValidator
    : AbstractValidator<DeleteAttendanceDeviceCommand>
{
    public DeleteAttendanceDeviceCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Attendance device ID is required.");
    }
}
