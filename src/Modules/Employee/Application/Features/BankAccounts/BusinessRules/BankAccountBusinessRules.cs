using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Employee.Domain.Entities;

namespace HRMS.Application.Features.BankAccounts.BusinessRules;

public class BankAccountBusinessRules
{
    private readonly IReadRepository<Employee, Guid> _employeeRepository;
    private readonly IReadRepository<BankAccount, Guid> _bankAccountRepository;

    public BankAccountBusinessRules(
        IReadRepository<Employee, Guid> employeeRepository,
        IReadRepository<BankAccount, Guid> bankAccountRepository)
    {
        _employeeRepository = employeeRepository;
        _bankAccountRepository = bankAccountRepository;
    }

    public async Task EnsureEmployeeExistsAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        var employee = await _employeeRepository.GetByIdAsync(
            employeeId,
            cancellationToken);

        if (employee is null)
        {
            throw new NotFoundException(
                "Employee",
                employeeId);
        }
    }

    public async Task EnsurePrimaryAccountAvailableAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _bankAccountRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.IsPrimary &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has a primary bank account.");
        }
    }

    public async Task EnsurePrimaryAccountAvailableAsync(
        Guid employeeId,
        Guid bankAccountId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _bankAccountRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.IsPrimary &&
                 x.Id != bankAccountId &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has a primary bank account.");
        }
    }
}
