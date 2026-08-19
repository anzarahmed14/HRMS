using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Modules.Leave.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddLeaveInfrastructure(
        this IServiceCollection services)
    {
        return services;
    }
}