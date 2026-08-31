using HRMS.Application.Features.Nominees.DTOs;
using MediatR;

namespace HRMS.Application.Features.Nominees.Queries.GetEmployeeNomineeById;

public record GetEmployeeNomineeByIdQuery(Guid Id)
    : IRequest<EmployeeNomineeDto?>;
