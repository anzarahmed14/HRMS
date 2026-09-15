using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.States.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.States.Commands.CreateState;

public sealed class CreateStateCommandHandler
    : IRequestHandler<CreateStateCommand, Guid>
{
    private readonly IWriteRepository<State, Guid> _repository;
    private readonly IReadRepository<Country, Guid> _countryRepository;
    private readonly StateBusinessRules _businessRules;

    public CreateStateCommandHandler(
        IWriteRepository<State, Guid> repository,
        IReadRepository<Country, Guid> countryRepository,
        StateBusinessRules businessRules)
    {
        _repository = repository;
        _countryRepository = countryRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateStateCommand request,
        CancellationToken cancellationToken)
    {
        var country = await _countryRepository.GetByIdAsync(
            request.CountryId,
            cancellationToken);

        if (country is null || country.IsDeleted)
        {
            throw new HRMS.BuildingBlocks.Application.Exceptions.NotFoundException(
                "Country",
                request.CountryId);
        }

        var code = request.Code.Trim();
        var name = request.Name.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            request.CountryId,
            code,
            cancellationToken);

        await _businessRules.EnsureNameUniqueAsync(
            request.CountryId,
            name,
            cancellationToken);

        var state = new State
        {
            Id = Guid.NewGuid(),
            CountryId = request.CountryId,
            Code = code,
            Name = name,
            IsActive = request.IsActive
        };

        await _repository.AddAsync(
            state,
            cancellationToken);

        return state.Id;
    }
}
