using HRMS.Application.Features.GovernmentIdentifiers;
using HRMS.Application.Features.GovernmentIdentifiers.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.GovernmentIdentifiers.Queries.GetGovernmentIdentifiers;

public sealed class GetGovernmentIdentifiersQueryHandler
    : IRequestHandler<
        GetGovernmentIdentifiersQuery,
        PagedResult<GovernmentIdentifierDto>>
{
    private readonly IReadRepository<EmployeeGovernmentIdentifier, Guid> _repository;

    public GetGovernmentIdentifiersQueryHandler(
        IReadRepository<EmployeeGovernmentIdentifier, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<GovernmentIdentifierDto>> Handle(
        GetGovernmentIdentifiersQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            predicate: x => x.EmployeeId == request.EmployeeId,
            cancellationToken: cancellationToken);

        return new PagedResult<GovernmentIdentifierDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new GovernmentIdentifierDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    IdentifierTypeId = x.IdentifierTypeId,
                    MaskedIdentifierNumber =
                        GovernmentIdentifierMasking.Mask(
                            x.IdentifierNumber),
                    IssueDate = x.IssueDate,
                    ExpiryDate = x.ExpiryDate,
                    IsVerified = x.IsVerified,
                    VerifiedOn = x.VerifiedOn
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
