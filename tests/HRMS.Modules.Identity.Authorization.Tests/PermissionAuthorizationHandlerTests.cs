using System.Linq.Expressions;
using System.Security.Claims;
using HRMS.BuildingBlocks.Application.Abstractions;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Identity.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Xunit;
// Aliased instead of a plain "using HRMS.Modules.Identity.Domain.Entities;"
// because this Identity module also contains unrelated, pre-existing
// scaffold namespaces "HRMS.Modules.User.*" (leftover Class1.cs template
// files), which otherwise make the bare "User" identifier ambiguous.
using User = HRMS.Modules.Identity.Domain.Entities.User;
using Role = HRMS.Modules.Identity.Domain.Entities.Role;
using UserRole = HRMS.Modules.Identity.Domain.Entities.UserRole;
using Permission = HRMS.Modules.Identity.Domain.Entities.Permission;
using RolePermission = HRMS.Modules.Identity.Domain.Entities.RolePermission;

namespace HRMS.Identity.Authorization.Tests;

public class PermissionAuthorizationHandlerTests
{
    private readonly Mock<IUserContext> _userContext = new();
    private readonly Mock<IReadRepository<User, Guid>> _userRepository = new();
    private readonly Mock<IReadRepository<Role, Guid>> _roleRepository = new();
    private readonly Mock<IReadRepository<Permission, Guid>> _permissionRepository = new();

    private PermissionAuthorizationHandler CreateHandler()
        => new(
            _userContext.Object,
            _userRepository.Object,
            _roleRepository.Object,
            _permissionRepository.Object);

    private static AuthorizationHandlerContext CreateContext(
        PermissionRequirement requirement)
        => new(
            new[] { requirement },
            new ClaimsPrincipal(new ClaimsIdentity()),
            resource: null);

    [Fact]
    public async Task Handle_WhenUserIsNotAuthenticated_DoesNotSucceed()
    {
        _userContext.SetupGet(x => x.UserId).Returns((Guid?)null);

        var handler = CreateHandler();
        var context = CreateContext(new PermissionRequirement("Employee.View"));

        await handler.HandleAsync(context);

        Assert.False(context.HasSucceeded);
        _userRepository.Verify(
            x => x.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<User, object>>[]>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenAuthenticatedUserHasThePermission_Succeeds()
    {
        var userId = Guid.NewGuid();
        var roleId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();

        _userContext.SetupGet(x => x.UserId).Returns(userId);

        var user = new User
        {
            Id = userId,
            UserRoles = new List<UserRole>
            {
                new() { UserId = userId, RoleId = roleId }
            }
        };

        _userRepository
            .Setup(x => x.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(user);

        var role = new Role
        {
            Id = roleId,
            IsActive = true,
            RolePermissions = new List<RolePermission>
            {
                new() { RoleId = roleId, PermissionId = permissionId }
            }
        };

        _roleRepository
            .Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<Role, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Role, object>>[]>()))
            .ReturnsAsync(new[] { role });

        var permission = new Permission
        {
            Id = permissionId,
            Name = "Employee.View",
            IsActive = true
        };

        _permissionRepository
            .Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<Permission, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { permission });

        var handler = CreateHandler();
        var context = CreateContext(new PermissionRequirement("Employee.View"));

        await handler.HandleAsync(context);

        Assert.True(context.HasSucceeded);
    }

    [Fact]
    public async Task Handle_WhenAuthenticatedUserLacksThePermission_DoesNotSucceed()
    {
        var userId = Guid.NewGuid();
        var roleId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();

        _userContext.SetupGet(x => x.UserId).Returns(userId);

        var user = new User
        {
            Id = userId,
            UserRoles = new List<UserRole>
            {
                new() { UserId = userId, RoleId = roleId }
            }
        };

        _userRepository
            .Setup(x => x.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(user);

        var role = new Role
        {
            Id = roleId,
            IsActive = true,
            RolePermissions = new List<RolePermission>
            {
                new() { RoleId = roleId, PermissionId = permissionId }
            }
        };

        _roleRepository
            .Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<Role, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Role, object>>[]>()))
            .ReturnsAsync(new[] { role });

        // The user only has "Employee.Create", not the requested
        // "Employee.View" — the requirement must not succeed.
        var permission = new Permission
        {
            Id = permissionId,
            Name = "Employee.Create",
            IsActive = true
        };

        _permissionRepository
            .Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<Permission, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { permission });

        var handler = CreateHandler();
        var context = CreateContext(new PermissionRequirement("Employee.View"));

        await handler.HandleAsync(context);

        Assert.False(context.HasSucceeded);
    }

    [Fact]
    public async Task Handle_ForUnknownFuturePermission_UsesSameDatabaseDrivenCheck()
    {
        // Proves the handler needs no code change for a permission that
        // doesn't exist in any policy registration (e.g. Payroll.Process)
        // — it is purely a database lookup by name.
        var userId = Guid.NewGuid();
        var roleId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();

        _userContext.SetupGet(x => x.UserId).Returns(userId);

        _userRepository
            .Setup(x => x.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(new User
            {
                Id = userId,
                UserRoles = new List<UserRole>
                {
                    new() { UserId = userId, RoleId = roleId }
                }
            });

        _roleRepository
            .Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<Role, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Role, object>>[]>()))
            .ReturnsAsync(new[]
            {
                new Role
                {
                    Id = roleId,
                    IsActive = true,
                    RolePermissions = new List<RolePermission>
                    {
                        new() { RoleId = roleId, PermissionId = permissionId }
                    }
                }
            });

        _permissionRepository
            .Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<Permission, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[]
            {
                new Permission
                {
                    Id = permissionId,
                    Name = "Payroll.Process",
                    IsActive = true
                }
            });

        var handler = CreateHandler();
        var context = CreateContext(new PermissionRequirement("Payroll.Process"));

        await handler.HandleAsync(context);

        Assert.True(context.HasSucceeded);
    }

    // ------------------------------------------------------------------
    // Phase 9 — additional authorization test matrix coverage.
    // ------------------------------------------------------------------

    [Fact]
    public async Task Handle_WhenAuthenticatedUserIdHasNoMatchingUserRecord_DoesNotSucceed()
    {
        // Covers "invalid user identity" — a JWT can carry a userId claim
        // for an account that no longer exists (or never did).
        var userId = Guid.NewGuid();
        _userContext.SetupGet(x => x.UserId).Returns(userId);

        _userRepository
            .Setup(x => x.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync((User?)null);

        var handler = CreateHandler();
        var context = CreateContext(new PermissionRequirement("Employee.View"));

        await handler.HandleAsync(context);

        Assert.False(context.HasSucceeded);
        _roleRepository.Verify(
            x => x.FindAsync(
                It.IsAny<Expression<Func<Role, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Role, object>>[]>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUserHasNoRoleAssignments_DoesNotSucceed()
    {
        var userId = Guid.NewGuid();
        _userContext.SetupGet(x => x.UserId).Returns(userId);

        _userRepository
            .Setup(x => x.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(new User { Id = userId, UserRoles = new List<UserRole>() });

        var handler = CreateHandler();
        var context = CreateContext(new PermissionRequirement("Employee.View"));

        await handler.HandleAsync(context);

        Assert.False(context.HasSucceeded);
        _roleRepository.Verify(
            x => x.FindAsync(
                It.IsAny<Expression<Func<Role, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Role, object>>[]>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenRoleHasNoPermissionAssignments_DoesNotSucceed()
    {
        var userId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        _userContext.SetupGet(x => x.UserId).Returns(userId);

        _userRepository
            .Setup(x => x.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(new User
            {
                Id = userId,
                UserRoles = new List<UserRole> { new() { UserId = userId, RoleId = roleId } }
            });

        _roleRepository
            .Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<Role, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Role, object>>[]>()))
            .ReturnsAsync(new[]
            {
                new Role { Id = roleId, IsActive = true, RolePermissions = new List<RolePermission>() }
            });

        var handler = CreateHandler();
        var context = CreateContext(new PermissionRequirement("Employee.View"));

        await handler.HandleAsync(context);

        Assert.False(context.HasSucceeded);
        _permissionRepository.Verify(
            x => x.FindAsync(
                It.IsAny<Expression<Func<Permission, bool>>>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUserHasThePermissionThroughOnlyOneOfTwoRoles_Succeeds()
    {
        var userId = Guid.NewGuid();
        var grantingRoleId = Guid.NewGuid();
        var otherRoleId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();

        _userContext.SetupGet(x => x.UserId).Returns(userId);

        _userRepository
            .Setup(x => x.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(new User
            {
                Id = userId,
                UserRoles = new List<UserRole>
                {
                    new() { UserId = userId, RoleId = grantingRoleId },
                    new() { UserId = userId, RoleId = otherRoleId }
                }
            });

        _roleRepository
            .Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<Role, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Role, object>>[]>()))
            .ReturnsAsync(new[]
            {
                new Role
                {
                    Id = grantingRoleId,
                    IsActive = true,
                    RolePermissions = new List<RolePermission>
                    {
                        new() { RoleId = grantingRoleId, PermissionId = permissionId }
                    }
                },
                new Role
                {
                    Id = otherRoleId,
                    IsActive = true,
                    RolePermissions = new List<RolePermission>()
                }
            });

        _permissionRepository
            .Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<Permission, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[]
            {
                new Permission { Id = permissionId, Name = "Employee.View", IsActive = true }
            });

        var handler = CreateHandler();
        var context = CreateContext(new PermissionRequirement("Employee.View"));

        await handler.HandleAsync(context);

        Assert.True(context.HasSucceeded);
    }

    [Fact]
    public async Task Handle_WhenOnlyTheGrantingRoleIsInactive_ExcludesItAndDoesNotSucceed()
    {
        // The handler's own role query filters on x.IsActive. This test
        // evaluates that real predicate against an in-memory role set
        // (rather than only recording that FindAsync was called), so it
        // proves the inactive role is actually excluded by the query the
        // handler builds — not just that IsActive appears somewhere.
        var userId = Guid.NewGuid();
        var inactiveRoleId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();

        var allRoles = new[]
        {
            new Role
            {
                Id = inactiveRoleId,
                IsActive = false,
                RolePermissions = new List<RolePermission>
                {
                    new() { RoleId = inactiveRoleId, PermissionId = permissionId }
                }
            }
        };

        _userContext.SetupGet(x => x.UserId).Returns(userId);

        _userRepository
            .Setup(x => x.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(new User
            {
                Id = userId,
                UserRoles = new List<UserRole>
                {
                    new() { UserId = userId, RoleId = inactiveRoleId }
                }
            });

        _roleRepository
            .Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<Role, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Role, object>>[]>()))
            .Returns<Expression<Func<Role, bool>>, CancellationToken, Expression<Func<Role, object>>[]>(
                (predicate, _, _) => Task.FromResult(allRoles.Where(predicate.Compile())));

        var handler = CreateHandler();
        var context = CreateContext(new PermissionRequirement("Employee.View"));

        await handler.HandleAsync(context);

        Assert.False(context.HasSucceeded);
        _permissionRepository.Verify(
            x => x.FindAsync(
                It.IsAny<Expression<Func<Permission, bool>>>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenMatchingPermissionRecordIsInactive_ExcludesItAndDoesNotSucceed()
    {
        // Same technique as the inactive-role test above, applied to the
        // handler's permission query — proves an inactive Permission row
        // (even with a matching name and an active granting role) is
        // excluded by the handler's own x.IsActive filter.
        var userId = Guid.NewGuid();
        var roleId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();

        var allPermissions = new[]
        {
            new Permission { Id = permissionId, Name = "Employee.View", IsActive = false }
        };

        _userContext.SetupGet(x => x.UserId).Returns(userId);

        _userRepository
            .Setup(x => x.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(new User
            {
                Id = userId,
                UserRoles = new List<UserRole> { new() { UserId = userId, RoleId = roleId } }
            });

        _roleRepository
            .Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<Role, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Role, object>>[]>()))
            .ReturnsAsync(new[]
            {
                new Role
                {
                    Id = roleId,
                    IsActive = true,
                    RolePermissions = new List<RolePermission>
                    {
                        new() { RoleId = roleId, PermissionId = permissionId }
                    }
                }
            });

        _permissionRepository
            .Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<Permission, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Permission, object>>[]>()))
            .Returns<Expression<Func<Permission, bool>>, CancellationToken, Expression<Func<Permission, object>>[]>(
                (predicate, _, _) => Task.FromResult(allPermissions.Where(predicate.Compile())));

        var handler = CreateHandler();
        var context = CreateContext(new PermissionRequirement("Employee.View"));

        await handler.HandleAsync(context);

        Assert.False(context.HasSucceeded);
    }

    [Fact]
    public async Task Handle_DoesNotCheckUserIsActive_InactiveUserWithValidRoleAndPermissionStillSucceeds()
    {
        // SECURITY FINDING (verified, not fixed in this phase — see Phase 9
        // report): PermissionAuthorizationHandler never reads User.IsActive.
        // A deactivated account with a still-valid JWT and an active role
        // retains full permission-based access until the token expires.
        // This test documents and locks in the CURRENT behavior; it must
        // not be "fixed" by silently changing the handler without separate
        // architectural approval, per the Phase 9 instructions.
        var userId = Guid.NewGuid();
        var roleId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();

        _userContext.SetupGet(x => x.UserId).Returns(userId);

        _userRepository
            .Setup(x => x.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(new User
            {
                Id = userId,
                IsActive = false, // deactivated account
                UserRoles = new List<UserRole> { new() { UserId = userId, RoleId = roleId } }
            });

        _roleRepository
            .Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<Role, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Role, object>>[]>()))
            .ReturnsAsync(new[]
            {
                new Role
                {
                    Id = roleId,
                    IsActive = true,
                    RolePermissions = new List<RolePermission>
                    {
                        new() { RoleId = roleId, PermissionId = permissionId }
                    }
                }
            });

        _permissionRepository
            .Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<Permission, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[]
            {
                new Permission { Id = permissionId, Name = "Employee.View", IsActive = true }
            });

        var handler = CreateHandler();
        var context = CreateContext(new PermissionRequirement("Employee.View"));

        await handler.HandleAsync(context);

        Assert.True(context.HasSucceeded);
    }
}
