using HRMS.Application.Features.BankAccounts.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.BankAccounts.Queries.GetBankAccounts;

public sealed class GetBankAccountsQueryHandler
    : IRequestHandler<
        GetBankAccountsQuery,
        PagedResult<BankAccountDto>>
{
    private readonly IReadRepository<BankAccount, Guid> _repository;

    public GetBankAccountsQueryHandler(
        IReadRepository<BankAccount, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<BankAccountDto>> Handle(
        GetBankAccountsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            predicate: x => x.EmployeeId == request.EmployeeId,
            cancellationToken: cancellationToken);

        return new PagedResult<BankAccountDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new BankAccountDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    AccountHolderName = x.AccountHolderName,
                    AccountNumber = BankAccountMasking.MaskAccountNumber(x.AccountNumber),
                    BankName = x.BankName,
                    IFSCCode = x.IFSCCode,
                    BranchName = x.BranchName,
                    AccountType = x.AccountType,
                    IsPrimary = x.IsPrimary
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
