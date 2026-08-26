using HRMS.Application.Features.BankAccounts.DTOs;
using MediatR;

namespace HRMS.Application.Features.BankAccounts.Queries.GetBankAccountById;

public record GetBankAccountByIdQuery(Guid Id)
    : IRequest<BankAccountDto?>;
