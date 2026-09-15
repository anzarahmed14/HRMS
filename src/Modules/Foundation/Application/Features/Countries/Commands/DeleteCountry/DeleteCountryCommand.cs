using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Countries.Commands.DeleteCountry;

public sealed record DeleteCountryCommand(
    Guid Id) : IRequest;
