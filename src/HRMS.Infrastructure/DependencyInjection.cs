using HRMS.Application.Common.Interfaces;
using HRMS.Infrastructure.Identity.Authorization;
using HRMS.Infrastructure.Identity.Jwt;
using HRMS.Infrastructure.Identity.Password;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure( this IServiceCollection services, IConfiguration configuration)
    {
        // Password hashing
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        // JWT configuration
        services.Configure<JwtOptions>( configuration.GetSection(JwtOptions.SectionName));

        // JWT token service
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddPermissionAuthorization();
        return services;
    }
}