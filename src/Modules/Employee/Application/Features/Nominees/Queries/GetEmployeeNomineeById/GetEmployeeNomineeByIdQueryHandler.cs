using AutoMapper;
using HRMS.Application.Features.Nominees.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Nominees.Queries.GetEmployeeNomineeById;

public class GetEmployeeNomineeByIdQueryHandler
    : IRequestHandler<GetEmployeeNomineeByIdQuery, EmployeeNomineeDto?>
{
    private readonly IReadRepository<EmployeeNominee, Guid> _repository;
    private readonly IMapper _mapper;

    public GetEmployeeNomineeByIdQueryHandler(
        IReadRepository<EmployeeNominee, Guid> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<EmployeeNomineeDto?> Handle(
        GetEmployeeNomineeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var nominee = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (nominee is null)
            return null;

        return _mapper.Map<EmployeeNomineeDto>(nominee);
    }
}
