using HRMS.Application.Features.BankAccounts.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.BankAccounts.Commands.DeleteBankAccount;

public class DeleteBankAccountCommandHandler
    : IRequestHandler<DeleteBankAccountCommand>
{
    private readonly IReadRepository<BankAccount, Guid> _readRepository;
    private readonly IWriteRepository<BankAccount, Guid> _writeRepository;
    private readonly BankAccountBusinessRules _businessRules;

    public DeleteBankAccountCommandHandler(
        IReadRepository<BankAccount, Guid> readRepository,
        IWriteRepository<BankAccount, Guid> writeRepository,
        BankAccountBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteBankAccountCommand request,
        CancellationToken cancellationToken)
    {
        var bankAccount = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (bankAccount is null)
        {
            throw new InvalidOperationException(
                "Bank account could not be loaded.");
        }

        await _businessRules.EnsureEmployeeExistsAsync(
            bankAccount.EmployeeId,
            cancellationToken);

        await _writeRepository.DeleteAsync(
            bankAccount,
            cancellationToken);
    }
}
