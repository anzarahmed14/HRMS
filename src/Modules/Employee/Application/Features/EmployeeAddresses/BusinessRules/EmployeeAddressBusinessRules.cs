using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Employee.Domain.Entities;
using HRMS.Modules.Foundation.Domain.Entities;

namespace HRMS.Application.Features.EmployeeAddresses.BusinessRules;

public class EmployeeAddressBusinessRules
{
    private readonly IReadRepository<Employee, Guid> _employeeRepository;
    private readonly IReadRepository<EmployeeAddress, Guid> _employeeAddressRepository;
    private readonly IReadRepository<AddressType, Guid> _addressTypeRepository;
    private readonly IReadRepository<Country, Guid> _countryRepository;
    private readonly IReadRepository<State, Guid> _stateRepository;

    public EmployeeAddressBusinessRules(
        IReadRepository<Employee, Guid> employeeRepository,
        IReadRepository<EmployeeAddress, Guid> employeeAddressRepository,
        IReadRepository<AddressType, Guid> addressTypeRepository,
        IReadRepository<Country, Guid> countryRepository,
        IReadRepository<State, Guid> stateRepository)
    {
        _employeeRepository = employeeRepository;
        _employeeAddressRepository = employeeAddressRepository;
        _addressTypeRepository = addressTypeRepository;
        _countryRepository = countryRepository;
        _stateRepository = stateRepository;
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

    public async Task EnsureAddressTypeExistsAsync(
        Guid addressTypeId,
        CancellationToken cancellationToken = default)
    {
        var addressType = await _addressTypeRepository.GetByIdAsync(
            addressTypeId,
            cancellationToken);

        if (addressType is null)
        {
            throw new NotFoundException(
                "AddressType",
                addressTypeId);
        }
    }

    public async Task EnsureCountryExistsAsync(
        Guid countryId,
        CancellationToken cancellationToken = default)
    {
        var country = await _countryRepository.GetByIdAsync(
            countryId,
            cancellationToken);

        if (country is null)
        {
            throw new NotFoundException(
                "Country",
                countryId);
        }
    }

    public async Task EnsureStateBelongsToCountryAsync(
        Guid stateId,
        Guid countryId,
        CancellationToken cancellationToken = default)
    {
        var state = await _stateRepository.GetByIdAsync(
            stateId,
            cancellationToken);

        if (state is null)
        {
            throw new NotFoundException(
                "State",
                stateId);
        }

        if (state.CountryId != countryId)
        {
            throw new ConflictException(
                "Selected state does not belong to the selected country.");
        }
    }

    public async Task EnsureAddressTypeAvailableAsync(
        Guid employeeId,
        Guid addressTypeId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _employeeAddressRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.AddressTypeId == addressTypeId &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has an address of this type.");
        }
    }

    public async Task EnsureAddressTypeAvailableAsync(
        Guid employeeId,
        Guid addressTypeId,
        Guid employeeAddressId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _employeeAddressRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.AddressTypeId == addressTypeId &&
                 x.Id != employeeAddressId &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has an address of this type.");
        }
    }

    public async Task EnsurePrimaryAddressAvailableAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _employeeAddressRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.IsPrimary &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has a primary address.");
        }
    }

    public async Task EnsurePrimaryAddressAvailableAsync(
        Guid employeeId,
        Guid employeeAddressId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _employeeAddressRepository.AnyAsync(
            x => x.EmployeeId == employeeId &&
                 x.IsPrimary &&
                 x.Id != employeeAddressId &&
                 !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "The employee already has a primary address.");
        }
    }
}
