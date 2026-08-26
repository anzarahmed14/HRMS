using AutoMapper;
using HRMS.Application.Features.BankAccounts.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.BankAccounts.Commands.UpdateBankAccount;

public class UpdateBankAccountCommandHandler
    : IRequestHandler<UpdateBankAccountCommand>
{
    private readonly IReadRepository<BankAccount, Guid> _readRepository;
    private readonly IWriteRepository<BankAccount, Guid> _writeRepository;
    private readonly BankAccountBusinessRules _businessRules;
    private readonly IMapper _mapper;

    public UpdateBankAccountCommandHandler(
        IReadRepository<BankAccount, Guid> readRepository,
        IWriteRepository<BankAccount, Guid> writeRepository,
        BankAccountBusinessRules businessRules,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
        _mapper = mapper;
    }

    public async Task Handle(
        UpdateBankAccountCommand request,
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
            request.EmployeeId,
            cancellationToken);

        if (request.IsPrimary)
        {
            await _businessRules.EnsurePrimaryAccountAvailableAsync(
                request.EmployeeId,
                request.Id,
                cancellationToken);
        }

        _mapper.Map(request, bankAccount);

        await _writeRepository.UpdateAsync(
            bankAccount,
            cancellationToken);
    }
}
