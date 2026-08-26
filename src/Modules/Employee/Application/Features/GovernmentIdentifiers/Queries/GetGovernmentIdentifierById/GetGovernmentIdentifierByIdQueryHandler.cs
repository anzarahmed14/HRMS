using HRMS.Application.Features.GovernmentIdentifiers;
using HRMS.Application.Features.GovernmentIdentifiers.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.GovernmentIdentifiers.Queries.GetGovernmentIdentifierById;

public class GetGovernmentIdentifierByIdQueryHandler
    : IRequestHandler<
        GetGovernmentIdentifierByIdQuery,
        GovernmentIdentifierDto?>
{
    private readonly IReadRepository<EmployeeGovernmentIdentifier, Guid> _repository;

    public GetGovernmentIdentifierByIdQueryHandler(
        IReadRepository<EmployeeGovernmentIdentifier, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<GovernmentIdentifierDto?> Handle(
        GetGovernmentIdentifierByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null)
            return null;

        return new GovernmentIdentifierDto
        {
            Id = entity.Id,
            EmployeeId = entity.EmployeeId,
            IdentifierTypeId = entity.IdentifierTypeId,
            MaskedIdentifierNumber =
                GovernmentIdentifierMasking.Mask(
                    entity.IdentifierNumber),
            IssueDate = entity.IssueDate,
            ExpiryDate = entity.ExpiryDate,
            IsVerified = entity.IsVerified,
            VerifiedOn = entity.VerifiedOn
        };
    }
}
