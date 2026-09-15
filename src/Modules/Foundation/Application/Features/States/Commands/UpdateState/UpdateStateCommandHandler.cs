using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Foundation.Application.Features.States.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.States.Commands.UpdateState;

public sealed class UpdateStateCommandHandler
    : IRequestHandler<UpdateStateCommand>
{
    private readonly IReadRepository<State, Guid> _readRepository;
    private readonly IWriteRepository<State, Guid> _writeRepository;
    private readonly IReadRepository<Country, Guid> _countryRepository;
    private readonly StateBusinessRules _businessRules;

    public UpdateStateCommandHandler(
        IReadRepository<State, Guid> readRepository,
        IWriteRepository<State, Guid> writeRepository,
        IReadRepository<Country, Guid> countryRepository,
        StateBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _countryRepository = countryRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateStateCommand request,
        CancellationToken cancellationToken)
    {
        var state = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (state is null || state.IsDeleted)
        {
            await _businessRules.EnsureStateExistsAsync(
                request.Id,
                cancellationToken);

            return;
        }

        var country = await _countryRepository.GetByIdAsync(
            request.CountryId,
            cancellationToken);

        if (country is null || country.IsDeleted)
        {
            throw new NotFoundException(
                "Country",
                request.CountryId);
        }

        var code = request.Code.Trim();
        var name = request.Name.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            request.CountryId,
            code,
            request.Id,
            cancellationToken);

        await _businessRules.EnsureNameUniqueAsync(
            request.CountryId,
            name,
            request.Id,
            cancellationToken);

        state.CountryId = request.CountryId;
        state.Code = code;
        state.Name = name;
        state.IsActive = request.IsActive;

        await _writeRepository.UpdateAsync(
            state,
            cancellationToken);
    }
}
