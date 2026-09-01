using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Identity.Application.Features.Identity.DTOs;
using HRMS.Modules.Identity.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Queries.GetUserById;

public class GetUserByIdQueryHandler
    : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IReadRepository<Domain.Entities.User, Guid> _repository;

    public GetUserByIdQueryHandler(
        IReadRepository<Domain.Entities.User, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<UserDto?> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (user is null)
        {
            return null;
        }

        return new UserDto
        {
            Id = user.Id,
            EmployeeId = user.EmployeeId,
            UserName = user.UserName,
            IsActive = user.IsActive
        };
    }
}
