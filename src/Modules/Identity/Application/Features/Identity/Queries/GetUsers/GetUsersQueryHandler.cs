using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Identity.Application.Features.Identity.DTOs;
using HRMS.Modules.Identity.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Queries.GetUsers;

public class GetUsersQueryHandler
    : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
{
    private readonly IReadRepository<HRMS.Modules.Identity.Domain.Entities.User, Guid> _repository;

    public GetUsersQueryHandler(
        IReadRepository<HRMS.Modules.Identity.Domain.Entities.User, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<UserDto>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var users = await _repository.GetAllAsync(
            cancellationToken);

        return users.Select(x => new UserDto
        {
            Id = x.Id,
            EmployeeId = x.EmployeeId,
            UserName = x.UserName,
            IsActive = x.IsActive
        });
    }
}
