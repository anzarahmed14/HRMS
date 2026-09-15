using FluentValidation;
using HRMS.Modules.Foundation.Application.Features.AddressTypes.BusinessRules;
using HRMS.Modules.Foundation.Application.Features.Certifications.BusinessRules;
using HRMS.Modules.Foundation.Application.Features.Countries.BusinessRules;
using HRMS.Modules.Foundation.Application.Features.DocumentTypes.BusinessRules;
using HRMS.Modules.Foundation.Application.Features.Genders.BusinessRules;
using HRMS.Modules.Foundation.Application.Features.IdentifierTypes.BusinessRules;
using HRMS.Modules.Foundation.Application.Features.Languages.BusinessRules;
using HRMS.Modules.Foundation.Application.Features.MaritalStatuses.BusinessRules;
using HRMS.Modules.Foundation.Application.Features.Relationships.BusinessRules;
using HRMS.Modules.Foundation.Application.Features.Skills.BusinessRules;
using HRMS.Modules.Foundation.Application.Features.States.BusinessRules;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HRMS.Modules.Foundation.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddFoundationApplication(
        this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(assembly));


        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(assembly);
        });

        services.AddValidatorsFromAssembly(assembly);

        services.AddScoped<GenderBusinessRules>();
        services.AddScoped<MaritalStatusBusinessRules>();
        services.AddScoped<CertificationBusinessRules>();
        services.AddScoped<IdentifierTypeBusinessRules>();
        services.AddScoped<DocumentTypeBusinessRules>();
        services.AddScoped<LanguageBusinessRules>();
        services.AddScoped<SkillBusinessRules>();
        services.AddScoped<RelationshipBusinessRules>();
        services.AddScoped<RelationshipBusinessRules>();
        services.AddScoped<StateBusinessRules>();
        services.AddScoped<AddressTypeBusinessRules>();
        services.AddScoped<CountryBusinessRules>();




        return services;
    }
}