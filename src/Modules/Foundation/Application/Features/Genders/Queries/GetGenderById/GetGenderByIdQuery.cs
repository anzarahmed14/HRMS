using HRMS.Modules.Foundation.Application.Features.Genders.DTOs;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Genders.Queries.GetGenderById;

public record GetGenderByIdQuery(
    Guid Id) : IRequest<GenderDto>;
