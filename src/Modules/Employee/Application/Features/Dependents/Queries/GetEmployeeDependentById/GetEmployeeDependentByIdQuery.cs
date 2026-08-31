using HRMS.Application.Features.Dependents.DTOs;
using MediatR;

namespace HRMS.Application.Features.Dependents.Queries.GetEmployeeDependentById;

public record GetEmployeeDependentByIdQuery(Guid Id)
    : IRequest<EmployeeDependentDto?>;
