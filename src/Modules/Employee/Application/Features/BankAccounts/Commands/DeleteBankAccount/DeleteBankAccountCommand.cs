using MediatR;

namespace HRMS.Application.Features.BankAccounts.Commands.DeleteBankAccount;

public record DeleteBankAccountCommand(Guid Id) : IRequest;
