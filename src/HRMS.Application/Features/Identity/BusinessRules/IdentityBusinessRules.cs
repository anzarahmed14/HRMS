using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces;
using HRMS.Shared.Exceptions;

namespace HRMS.Application.Features.Identity.BusinessRules;

public class IdentityBusinessRules
{
    private readonly IReadRepository<Employee, Guid> _employeeRepository;
    private readonly IReadRepository<User, Guid> _userRepository;

    public async Task<User> EnsureUserCanLoginAsync(
        string userName,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.FirstOrDefaultAsync(
            x => x.UserName == userName,
            cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedException(
                "Invalid username or password.");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedException(
                "User account is inactive.");
        }

        return user;
    }

    public IdentityBusinessRules(
        IReadRepository<Employee, Guid> employeeRepository,
        IReadRepository<User, Guid> userRepository)
    {
        _employeeRepository = employeeRepository;
        _userRepository = userRepository;
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

    public async Task EnsureEmployeeDoesNotHaveUserAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _userRepository.AnyAsync(
            x => x.EmployeeId == employeeId,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "This employee already has a user account.");
        }
    }

    public async Task EnsureUserNameUniqueAsync(
        string userName,
        CancellationToken cancellationToken = default)
    {
        var exists = await _userRepository.AnyAsync(
            x => x.UserName == userName,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Username already exists.");
        }
    }
}