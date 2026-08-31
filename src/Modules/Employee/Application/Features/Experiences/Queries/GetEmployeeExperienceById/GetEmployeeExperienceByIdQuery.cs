using HRMS.Application.Features.Experiences.DTOs;
using MediatR;

namespace HRMS.Application.Features.Experiences.Queries.GetEmployeeExperienceById;

public record GetEmployeeExperienceByIdQuery(Guid Id)
    : IRequest<EmployeeExperienceDto?>;
