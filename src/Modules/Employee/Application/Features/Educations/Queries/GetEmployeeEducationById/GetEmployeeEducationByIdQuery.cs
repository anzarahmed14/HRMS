using HRMS.Application.Features.Educations.DTOs;
using MediatR;

namespace HRMS.Application.Features.Educations.Queries.GetEmployeeEducationById;

public record GetEmployeeEducationByIdQuery(Guid Id)
    : IRequest<EmployeeEducationDto?>;
