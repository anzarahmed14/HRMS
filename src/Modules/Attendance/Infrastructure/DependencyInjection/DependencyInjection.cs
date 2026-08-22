using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Modules.Attendance.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddAttendanceInfrastructure(
        this IServiceCollection services)
    {
        return services;
    }
}
