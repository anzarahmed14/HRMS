using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.Countries.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Countries.Commands.CreateCountry;

public sealed class CreateCountryCommandHandler
    : IRequestHandler<CreateCountryCommand, Guid>
{
    private readonly IWriteRepository<Country, Guid> _repository;
    private readonly CountryBusinessRules _businessRules;

    public CreateCountryCommandHandler(
        IWriteRepository<Country, Guid> repository,
        CountryBusinessRules businessRules)
    {
        _repository = repository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateCountryCommand request,
        CancellationToken cancellationToken)
    {
        var code = request.Code.Trim();
        var name = request.Name.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            code,
            cancellationToken);

        await _businessRules.EnsureNameUniqueAsync(
            name,
            cancellationToken);

        var country = new Country
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = name,
            IsActive = request.IsActive
        };

        await _repository.AddAsync(
            country,
            cancellationToken);

        return country.Id;
    }
}
