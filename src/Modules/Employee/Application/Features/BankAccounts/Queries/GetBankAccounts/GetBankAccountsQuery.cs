using HRMS.Application.Features.BankAccounts.DTOs;
using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;

namespace HRMS.Application.Features.BankAccounts.Queries.GetBankAccounts;

public sealed record GetBankAccountsQuery(
    Guid EmployeeId,
    PagedRequest Request
) : IRequest<PagedResult<BankAccountDto>>;
