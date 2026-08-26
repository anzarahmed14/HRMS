using AutoMapper;
using HRMS.Application.Features.EmployeeAddresses.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmployeeAddresses.Queries.GetEmployeeAddressById;

public class GetEmployeeAddressByIdQueryHandler
    : IRequestHandler<GetEmployeeAddressByIdQuery, EmployeeAddressDto?>
{
    private readonly IReadRepository<EmployeeAddress, Guid> _repository;
    private readonly IMapper _mapper;

    public GetEmployeeAddressByIdQueryHandler(
        IReadRepository<EmployeeAddress, Guid> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<EmployeeAddressDto?> Handle(
        GetEmployeeAddressByIdQuery request,
        CancellationToken cancellationToken)
    {
        var employeeAddress = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (employeeAddress is null)
            return null;

        return _mapper.Map<EmployeeAddressDto>(
            employeeAddress);
    }
}
