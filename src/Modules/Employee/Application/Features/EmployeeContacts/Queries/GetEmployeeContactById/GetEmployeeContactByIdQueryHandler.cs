using AutoMapper;
using HRMS.Application.Features.EmployeeContacts.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmployeeContacts.Queries.GetEmployeeContactById;

public class GetEmployeeContactByIdQueryHandler
    : IRequestHandler<GetEmployeeContactByIdQuery, EmployeeContactDto?>
{
    private readonly IReadRepository<EmployeeContact, Guid> _repository;
    private readonly IMapper _mapper;

    public GetEmployeeContactByIdQueryHandler(
        IReadRepository<EmployeeContact, Guid> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<EmployeeContactDto?> Handle(
        GetEmployeeContactByIdQuery request,
        CancellationToken cancellationToken)
    {
        var employeeContact = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (employeeContact is null)
            return null;

        return _mapper.Map<EmployeeContactDto>(
            employeeContact);
    }
}
