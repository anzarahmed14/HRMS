using AutoMapper;
using HRMS.Application.Features.Dependents.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Dependents.Queries.GetEmployeeDependentById;

public class GetEmployeeDependentByIdQueryHandler
    : IRequestHandler<
        GetEmployeeDependentByIdQuery,
        EmployeeDependentDto?>
{
    private readonly IReadRepository<EmployeeDependent, Guid> _repository;
    private readonly IMapper _mapper;

    public GetEmployeeDependentByIdQueryHandler(
        IReadRepository<EmployeeDependent, Guid> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<EmployeeDependentDto?> Handle(
        GetEmployeeDependentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var dependent = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (dependent is null)
            return null;

        return _mapper.Map<EmployeeDependentDto>(
            dependent);
    }
}
