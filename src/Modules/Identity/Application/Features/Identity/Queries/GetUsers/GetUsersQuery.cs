using HRMS.Modules.Identity.Application.Features.Identity.DTOs;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Queries.GetUsers;

public record GetUsersQuery : IRequest<IEnumerable<UserDto>>;
