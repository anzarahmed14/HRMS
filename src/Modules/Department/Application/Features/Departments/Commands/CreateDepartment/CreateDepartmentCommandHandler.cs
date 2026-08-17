using AutoMapper;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Department.Application.Features.Departments.BusinessRules;
using HRMS.Modules.Department.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Department.Application.Features.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandHandler
    : IRequestHandler<CreateDepartmentCommand, Guid>
{
    private readonly IWriteRepository<HRMS.Modules.Department.Domain.Entities.Department, Guid> _writeRepository;
    private readonly DepartmentBusinessRules _departmentRules;
    private readonly IMapper _mapper;

    public CreateDepartmentCommandHandler(
        IWriteRepository<HRMS.Modules.Department.Domain.Entities.Department, Guid> writeRepository,
        DepartmentBusinessRules departmentRules,
        IMapper mapper)
    {
        _writeRepository = writeRepository;
        _departmentRules = departmentRules;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(
        CreateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        await _departmentRules.EnsureDepartmentNameUniqueAsync(
            request.Name,
            cancellationToken);

        var department = _mapper.Map<  Domain.Entities.Department>(request);

        await _writeRepository.AddAsync(
            department,
            cancellationToken);

        return department.Id;
    }
}