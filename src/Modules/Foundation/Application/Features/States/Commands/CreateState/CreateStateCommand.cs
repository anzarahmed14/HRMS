using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.States.Commands.CreateState;

public sealed record CreateStateCommand(
    Guid CountryId,
    string Code,
    string Name,
    bool IsActive = true) : IRequest<Guid>;
