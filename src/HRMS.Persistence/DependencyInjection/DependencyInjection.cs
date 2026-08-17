using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Persistence.Context;
using HRMS.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Persistence.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped(typeof(IReadRepository<,>), typeof(BaseReadRepository<,>));

        services.AddScoped(typeof(IWriteRepository<,>), typeof(BaseWriteRepository<,>));


        return services;
    }
}

