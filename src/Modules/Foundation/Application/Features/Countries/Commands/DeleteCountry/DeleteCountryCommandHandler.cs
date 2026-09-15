using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.Countries.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Countries.Commands.DeleteCountry;

public sealed class DeleteCountryCommandHandler
    : IRequestHandler<DeleteCountryCommand>
{
    private readonly IReadRepository<Country, Guid> _readRepository;
    private readonly IWriteRepository<Country, Guid> _writeRepository;
    private readonly CountryBusinessRules _businessRules;

    public DeleteCountryCommandHandler(
        IReadRepository<Country, Guid> readRepository,
        IWriteRepository<Country, Guid> writeRepository,
        CountryBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteCountryCommand request,
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

        await _writeRepository.DeleteAsync(
            country,
            cancellationToken);
    }
}
