using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.Countries.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Countries.Commands.UpdateCountry;

public sealed class UpdateCountryCommandHandler
    : IRequestHandler<UpdateCountryCommand>
{
    private readonly IReadRepository<Country, Guid> _readRepository;
    private readonly IWriteRepository<Country, Guid> _writeRepository;
    private readonly CountryBusinessRules _businessRules;

    public UpdateCountryCommandHandler(
        IReadRepository<Country, Guid> readRepository,
        IWriteRepository<Country, Guid> writeRepository,
        CountryBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateCountryCommand request,
        CancellationToken cancellationToken)
    {
        var country = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (country is null || country.IsDeleted)
        {
            await _businessRules.EnsureCountryExistsAsync(
                request.Id,
                cancellationToken);

            return;
        }

        var code = request.Code.Trim();
        var name = request.Name.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            code,
            request.Id,
            cancellationToken);

        await _businessRules.EnsureNameUniqueAsync(
            name,
            request.Id,
            cancellationToken);

        country.Code = code;
        country.Name = name;
        country.IsActive = request.IsActive;

        await _writeRepository.UpdateAsync(
            country,
            cancellationToken);
    }
}
