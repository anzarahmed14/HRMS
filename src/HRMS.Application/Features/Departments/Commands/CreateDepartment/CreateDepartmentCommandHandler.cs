using AutoMapper;
using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces;
using MediatR;

namespace HRMS.Application.Features.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandHandler
    : IRequestHandler<CreateDepartmentCommand, Guid>
{
    private readonly IReadRepository<Department, Guid> _readRepository;
    private readonly IWriteRepository<Department, Guid> _writeRepository;
    private readonly IMapper _mapper;

    public CreateDepartmentCommandHandler(
        IReadRepository<Department, Guid> readRepository,
        IWriteRepository<Department, Guid> writeRepository,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(
        CreateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await _readRepository.AnyAsync(
            x => x.Name == request.Name,
            cancellationToken);

        if (exists)
            throw new Exception("Department already exists.");

        var department = _mapper.Map<Department>(request);

        await _writeRepository.AddAsync(
            department,
            cancellationToken);

        return department.Id;
    }
}