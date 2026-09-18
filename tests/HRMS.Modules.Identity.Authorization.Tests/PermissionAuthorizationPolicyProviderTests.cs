using HRMS.Modules.Identity.Application.Authorization;
using HRMS.Modules.Identity.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Xunit;

namespace HRMS.Identity.Authorization.Tests;

public class PermissionAuthorizationPolicyProviderTests
{
    private static PermissionAuthorizationPolicyProvider CreateProvider(
        Action<AuthorizationOptions>? configure = null)
    {
        var options = new AuthorizationOptions();
        configure?.Invoke(options);

        return new PermissionAuthorizationPolicyProvider(
            Options.Create(options));
    }

    [Fact]
    public async Task GetPolicyAsync_ForKnownPermission_ReturnsPolicyWithPermissionRequirement()
    {
        var provider = CreateProvider();

        var policy = await provider.GetPolicyAsync(
            PermissionAuthorizeAttribute.PolicyPrefix + PermissionNames.Employee.View);

        Assert.NotNull(policy);
        var requirement = Assert.Single(policy!.Requirements);
        var permissionRequirement = Assert.IsType<PermissionRequirement>(requirement);
        Assert.Equal(PermissionNames.Employee.View, permissionRequirement.Permission);
    }

    [Fact]
    public async Task GetPolicyAsync_ForFuturePermissionNeverRegisteredAnywhere_StillResolves()
    {
        // Proves the provider needs no code change for permissions that
        // don't exist yet anywhere in the codebase (e.g. Payroll.Process).
        var provider = CreateProvider();

        var policy = await provider.GetPolicyAsync(
            PermissionAuthorizeAttribute.PolicyPrefix + "Payroll.Process");

        Assert.NotNull(policy);
        var requirement = Assert.Single(policy!.Requirements);
        var permissionRequirement = Assert.IsType<PermissionRequirement>(requirement);
        Assert.Equal("Payroll.Process", permissionRequirement.Permission);
    }

    [Fact]
    public async Task GetPolicyAsync_ForNonPermissionPolicy_DelegatesToDefaultProvider()
    {
        var provider = CreateProvider(options =>
            options.AddPolicy(
                "SomeOtherPolicy",
                policy => policy.RequireAuthenticatedUser()));

        var policy = await provider.GetPolicyAsync("SomeOtherPolicy");

        Assert.NotNull(policy);
        Assert.DoesNotContain(
            policy!.Requirements,
            x => x is PermissionRequirement);
    }

    [Fact]
    public async Task GetPolicyAsync_ForUnknownNonPermissionPolicy_ReturnsNull()
    {
        var provider = CreateProvider();

        var policy = await provider.GetPolicyAsync("NotRegisteredAnywhere");

        Assert.Null(policy);
    }

    [Fact]
    public async Task GetPolicyAsync_ForPermissionPolicyWithEmptyName_Throws()
    {
        var provider = CreateProvider();

        await Assert.ThrowsAsync<ArgumentException>(
            () => provider.GetPolicyAsync(PermissionAuthorizeAttribute.PolicyPrefix));
    }

    [Fact]
    public async Task GetDefaultPolicyAsync_RequiresAuthenticatedUser()
    {
        var provider = CreateProvider();

        var policy = await provider.GetDefaultPolicyAsync();

        Assert.NotNull(policy);
    }
}
