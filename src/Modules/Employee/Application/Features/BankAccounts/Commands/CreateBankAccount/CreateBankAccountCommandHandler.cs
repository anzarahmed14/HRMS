using HRMS.Application.Features.BankAccounts.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.BankAccounts.Commands.CreateBankAccount;

public class CreateBankAccountCommandHandler
    : IRequestHandler<CreateBankAccountCommand, Guid>
{
    private readonly IWriteRepository<BankAccount, Guid> _writeRepository;
    private readonly BankAccountBusinessRules _businessRules;

    public CreateBankAccountCommandHandler(
        IWriteRepository<BankAccount, Guid> writeRepository,
        BankAccountBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateBankAccountCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        if (request.IsPrimary)
        {
            await _businessRules.EnsurePrimaryAccountAvailableAsync(
                request.EmployeeId,
                cancellationToken);
        }

        var bankAccount = new BankAccount
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            AccountHolderName = request.AccountHolderName,
            AccountNumber = request.AccountNumber,
            BankName = request.BankName,
            IFSCCode = request.IFSCCode,
            BranchName = request.BranchName,
            AccountType = request.AccountType,
            IsPrimary = request.IsPrimary
        };

        await _writeRepository.AddAsync(
            bankAccount,
            cancellationToken);

        return bankAccount.Id;
    }
}
