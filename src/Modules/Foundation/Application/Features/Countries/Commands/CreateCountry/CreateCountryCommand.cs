using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Countries.Commands.CreateCountry;

public sealed record CreateCountryCommand(
    string Code,
    string Name,
    bool IsActive = true) : IRequest<Guid>;
