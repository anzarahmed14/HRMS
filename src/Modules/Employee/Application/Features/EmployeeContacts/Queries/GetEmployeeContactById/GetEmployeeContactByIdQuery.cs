using HRMS.Application.Features.EmployeeContacts.DTOs;
using MediatR;

namespace HRMS.Application.Features.EmployeeContacts.Queries.GetEmployeeContactById;

public record GetEmployeeContactByIdQuery(Guid Id)
    : IRequest<EmployeeContactDto?>;
