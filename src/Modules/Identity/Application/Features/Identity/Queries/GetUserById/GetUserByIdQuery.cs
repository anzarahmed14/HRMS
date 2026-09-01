using HRMS.Modules.Identity.Application.Features.Identity.DTOs;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id)
    : IRequest<UserDto?>;
