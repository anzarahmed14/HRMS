using HRMS.Domain.Interfaces;
using HRMS.Persistence.Context;
using HRMS.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Persistence.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

       // services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        
        services.AddScoped(typeof(IReadRepository<,>), typeof(BaseReadRepository<,>));
        services.AddScoped(typeof(IWriteRepository<,>), typeof(BaseWriteRepository<,>));

        return services;
    }
}