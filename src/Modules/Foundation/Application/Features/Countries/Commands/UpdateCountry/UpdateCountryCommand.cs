using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Countries.Commands.UpdateCountry;

public sealed record UpdateCountryCommand(
    Guid Id,
    string Code,
    string Name,
    bool IsActive) : IRequest;
