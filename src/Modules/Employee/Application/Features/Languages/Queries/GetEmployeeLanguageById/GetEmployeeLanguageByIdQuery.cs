using HRMS.Application.Features.Languages.DTOs;
using MediatR;

namespace HRMS.Application.Features.Languages.Queries.GetEmployeeLanguageById;

public record GetEmployeeLanguageByIdQuery(Guid Id)
    : IRequest<EmployeeLanguageDto?>;
