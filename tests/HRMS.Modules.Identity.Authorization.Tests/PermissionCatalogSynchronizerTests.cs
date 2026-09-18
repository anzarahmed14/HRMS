using System.Linq.Expressions;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Identity.Application.Authorization;
using HRMS.Modules.Identity.Infrastructure.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
// Aliased for the same reason as PermissionAuthorizationHandlerTests — the
// Identity module also contains unrelated, pre-existing scaffold
// namespaces "HRMS.Modules.User.*" that otherwise make "Module" and
// "Permission" ambiguous.
using Module = HRMS.Modules.Identity.Domain.Entities.Module;
using Permission = HRMS.Modules.Identity.Domain.Entities.Permission;

namespace HRMS.Identity.Authorization.Tests;

public class PermissionCatalogSynchronizerTests
{
    private readonly Mock<IReadRepository<Module, Guid>> _moduleReadRepository = new();
    private readonly Mock<IReadRepository<Permission, Guid>> _permissionReadRepository = new();
    private readonly Mock<IWriteRepository<Permission, Guid>> _permissionWriteRepository = new();

    private static readonly Guid IdentityModuleId = Guid.NewGuid();
    private static readonly Guid EmployeeModuleId = Guid.NewGuid();
    private static readonly Guid DepartmentModuleId = Guid.NewGuid();

    private PermissionCatalogSynchronizer CreateSynchronizer()
    {
        _moduleReadRepository
            .Setup(x => x.GetAllAsync(
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Module, object>>[]>()))
            .ReturnsAsync(new[]
            {
                new Module { Id = IdentityModuleId, Code = "IDENTITY", Name = "Identity" },
                new Module { Id = EmployeeModuleId, Code = "EMPLOYEE", Name = "Employee" },
                new Module { Id = DepartmentModuleId, Code = "DEPARTMENT", Name = "Department" }
            });

        return new PermissionCatalogSynchronizer(
            _moduleReadRepository.Object,
            _permissionReadRepository.Object,
            _permissionWriteRepository.Object,
            Mock.Of<ILogger<PermissionCatalogSynchronizer>>());
    }

    private void SetUpExistingPermissions(params Permission[] existing)
    {
        _permissionReadRepository
            .Setup(x => x.GetAllAsync(
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Permission, object>>[]>()))
            .ReturnsAsync(existing);
    }

    [Fact]
    public async Task SynchronizeAsync_WhenPermissionIsMissing_CreatesIt()
    {
        SetUpExistingPermissions();

        var synchronizer = CreateSynchronizer();

        var result = await synchronizer.SynchronizeAsync();

        Assert.Equal(
            PermissionCatalog.Entries.Count,
            result.Created.Count);
        Assert.Contains(PermissionNames.Department.View, result.Created);

        _permissionWriteRepository.Verify(
            x => x.AddRangeAsync(
                It.Is<IEnumerable<Permission>>(p =>
                    p.Count() == PermissionCatalog.Entries.Count),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task SynchronizeAsync_WhenPermissionAlreadyExists_LeavesItUnchangedAndDoesNotRecreateIt()
    {
        var existing = new Permission
        {
            Id = Guid.NewGuid(),
            Name = PermissionNames.Employee.View,
            ModuleId = EmployeeModuleId,
            IsActive = false // deliberately deactivated — must stay untouched
        };

        SetUpExistingPermissions(existing);

        var synchronizer = CreateSynchronizer();

        var result = await synchronizer.SynchronizeAsync();

        Assert.Contains(PermissionNames.Employee.View, result.AlreadyPresent);
        Assert.DoesNotContain(PermissionNames.Employee.View, result.Created);

        // The existing row itself must never be passed to a write call.
        _permissionWriteRepository.Verify(
            x => x.AddRangeAsync(
                It.Is<IEnumerable<Permission>>(p =>
                    !p.Any(x => x.Name == PermissionNames.Employee.View)),
                It.IsAny<CancellationToken>()),
            Times.Once);

        Assert.False(existing.IsActive);
    }

    [Fact]
    public async Task SynchronizeAsync_WhenCatalogIsFullyPresent_IsIdempotentAndWritesNothing()
    {
        var existing = PermissionCatalog.Entries
            .Select(e => new Permission
            {
                Id = Guid.NewGuid(),
                Name = e.Name,
                ModuleId = Guid.NewGuid(),
                IsActive = true
            })
            .ToArray();

        SetUpExistingPermissions(existing);

        var synchronizer = CreateSynchronizer();

        var result = await synchronizer.SynchronizeAsync();

        Assert.Empty(result.Created);
        Assert.Equal(PermissionCatalog.Entries.Count, result.AlreadyPresent.Count);

        _permissionWriteRepository.Verify(
            x => x.AddRangeAsync(
                It.IsAny<IEnumerable<Permission>>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task SynchronizeAsync_AssignsTheCorrectModuleIdForEachCreatedPermission()
    {
        SetUpExistingPermissions();

        var synchronizer = CreateSynchronizer();

        Permission[]? created = null;

        _permissionWriteRepository
            .Setup(x => x.AddRangeAsync(
                It.IsAny<IEnumerable<Permission>>(),
                It.IsAny<CancellationToken>()))
            .Callback<IEnumerable<Permission>, CancellationToken>(
                (permissions, _) => created = permissions.ToArray());

        await synchronizer.SynchronizeAsync();

        Assert.NotNull(created);
        Assert.Equal(
            IdentityModuleId,
            created!.Single(x => x.Name == PermissionNames.User.ResetPassword).ModuleId);
        Assert.Equal(
            EmployeeModuleId,
            created.Single(x => x.Name == PermissionNames.Employee.View).ModuleId);
        Assert.Equal(
            DepartmentModuleId,
            created.Single(x => x.Name == PermissionNames.Department.Create).ModuleId);
    }

    [Fact]
    public async Task SynchronizeAsync_WhenExistingPermissionIsNotInCatalog_ReportsItAsOrphanedWithoutTouchingIt()
    {
        var stale = new Permission
        {
            Id = Guid.NewGuid(),
            Name = "Verify.TempPermission",
            ModuleId = EmployeeModuleId,
            IsActive = true
        };

        SetUpExistingPermissions(stale);

        var synchronizer = CreateSynchronizer();

        var result = await synchronizer.SynchronizeAsync();

        Assert.Contains("Verify.TempPermission", result.Orphaned);

        _permissionWriteRepository.Verify(
            x => x.DeleteAsync(It.IsAny<Permission>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _permissionWriteRepository.Verify(
            x => x.UpdateAsync(It.IsAny<Permission>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task SynchronizeAsync_WhenCatalogEntryHasNoMatchingModule_SkipsItAndReportsUnresolved()
    {
        // No modules registered at all — every catalog entry's module code
        // fails to resolve.
        _moduleReadRepository
            .Setup(x => x.GetAllAsync(
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Module, object>>[]>()))
            .ReturnsAsync(Array.Empty<Module>());

        SetUpExistingPermissions();

        var synchronizer = new PermissionCatalogSynchronizer(
            _moduleReadRepository.Object,
            _permissionReadRepository.Object,
            _permissionWriteRepository.Object,
            Mock.Of<ILogger<PermissionCatalogSynchronizer>>());

        var result = await synchronizer.SynchronizeAsync();

        Assert.Empty(result.Created);
        Assert.Equal(PermissionCatalog.Entries.Count, result.Unresolved.Count);

        _permissionWriteRepository.Verify(
            x => x.AddRangeAsync(
                It.IsAny<IEnumerable<Permission>>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
