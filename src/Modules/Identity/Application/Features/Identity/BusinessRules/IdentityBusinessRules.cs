using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Identity.Domain.Entities;

namespace HRMS.Modules.Identity.Application.Features.Identity.BusinessRules;

public class IdentityBusinessRules
{
    private readonly IReadRepository<
        HRMS.Modules.Employee.Domain.Entities.Employee,
        Guid> _employeeRepository;

    private readonly IReadRepository<
        HRMS.Modules.Identity.Domain.Entities.User,
        Guid> _userRepository;

    private readonly IReadRepository<Role, Guid> _roleRepository;

    public IdentityBusinessRules(
        IReadRepository<
            HRMS.Modules.Employee.Domain.Entities.Employee,
            Guid> employeeRepository,

        IReadRepository<
            HRMS.Modules.Identity.Domain.Entities.User,
            Guid> userRepository,

        IReadRepository<Role, Guid> roleRepository)
    {
        _employeeRepository = employeeRepository;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<
        HRMS.Modules.Identity.Domain.Entities.User>
        EnsureUserCanLoginAsync(
            string userName,
            CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.FirstOrDefaultAsync(
            x => x.UserName == userName,
            cancellationToken,
            x => x.UserRoles);

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

        var roleIds = user.UserRoles
            .Select(x => x.RoleId)
            .ToList();

        if (roleIds.Count == 0)
        {
            throw new UnauthorizedException(
                "User has no role assigned.");
        }

        var roles = await _roleRepository.FindAsync(
            x => roleIds.Contains(x.Id),
            cancellationToken);

        foreach (var userRole in user.UserRoles)
        {
            var role = roles.FirstOrDefault(
                x => x.Id == userRole.RoleId);

            if (role is not null)
            {
                userRole.Role = role;
            }
        }

        return user;
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

    public async Task<
        HRMS.Modules.Identity.Domain.Entities.User>
        EnsureUserExistsAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(
            userId,
            cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(
                "User",
                userId);
        }

        return user;
    }

    public async Task EnsureUserNameUniqueAsync(
        string userName,
        Guid excludeUserId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _userRepository.AnyAsync(
            x => x.UserName == userName &&
                 x.Id != excludeUserId,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Username already exists.");
        }
    }
    public async Task<Role> EnsureRoleExistsAsync(
    Guid roleId,
    CancellationToken cancellationToken = default)
    {
        var role = await _roleRepository.GetByIdAsync(
            roleId,
            cancellationToken);

        if (role is null)
        {
            throw new NotFoundException(
                "Role",
                roleId);
        }

        return role;
    }

    public async Task EnsureRoleNameUniqueAsync(
        string roleName,
        CancellationToken cancellationToken = default)
    {
        var exists = await _roleRepository.AnyAsync(
            x => x.Name == roleName,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Role name already exists.");
        }
    }

    public async Task EnsureRoleNameUniqueAsync(
        string roleName,
        Guid excludeRoleId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _roleRepository.AnyAsync(
            x => x.Name == roleName &&
                 x.Id != excludeRoleId,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Role name already exists.");
        }
    }
    public async Task EnsureUserRoleDoesNotExistAsync(
    Guid userId,
    Guid roleId,
    CancellationToken cancellationToken = default)
    {
        var exists = await _userRepository.AnyAsync(
            x => x.Id == userId &&
                 x.UserRoles.Any(ur => ur.RoleId == roleId),
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "This role is already assigned to the user.");
        }
    }
}