using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Foundation.Application.Features.States.DTOs;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.States.Queries.GetStateById;

public sealed class GetStateByIdQueryHandler
    : IRequestHandler<GetStateByIdQuery, StateDto>
{
    private readonly IReadRepository<State, Guid> _repository;

    public GetStateByIdQueryHandler(
        IReadRepository<State, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<StateDto> Handle(
        GetStateByIdQuery request,
        CancellationToken cancellationToken)
    {
        var state = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (state is null || state.IsDeleted)
        {
            throw new NotFoundException(
                "State",
                request.Id);
        }

        return new StateDto
        {
            Id = state.Id,
            CountryId = state.CountryId,
            Code = state.Code,
            Name = state.Name,
            IsActive = state.IsActive
        };
    }
}
