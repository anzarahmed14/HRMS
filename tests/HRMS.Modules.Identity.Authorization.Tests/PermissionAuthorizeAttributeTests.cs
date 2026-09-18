using HRMS.Modules.Identity.Application.Authorization;
using Xunit;

namespace HRMS.Identity.Authorization.Tests;

public class PermissionAuthorizeAttributeTests
{
    [Fact]
    public void Constructor_SetsPolicyToPrefixedPermissionName()
    {
        var attribute = new PermissionAuthorizeAttribute(PermissionNames.Employee.View);

        Assert.Equal(PermissionNames.Employee.View, attribute.Permission);
        Assert.Equal(
            PermissionAuthorizeAttribute.PolicyPrefix + PermissionNames.Employee.View,
            attribute.Policy);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithNullOrEmptyPermission_Throws(string? permission)
    {
        Assert.Throws<ArgumentException>(
            () => new PermissionAuthorizeAttribute(permission!));
    }
}
