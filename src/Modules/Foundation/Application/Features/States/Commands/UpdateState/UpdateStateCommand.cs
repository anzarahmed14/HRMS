using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.States.Commands.UpdateState;

public sealed record UpdateStateCommand(
    Guid Id,
    Guid CountryId,
    string Code,
    string Name,
    bool IsActive) : IRequest;
