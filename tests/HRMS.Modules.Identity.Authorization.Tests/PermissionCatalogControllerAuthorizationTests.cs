using System.Reflection;
using HRMS.API.Controllers.Identity;
using Microsoft.AspNetCore.Authorization;
using Xunit;

namespace HRMS.Identity.Authorization.Tests;

/// <summary>
/// Verifies that <see cref="PermissionCatalogController"/> declares the
/// authorization it is expected to enforce.
/// </summary>
/// <remarks>
/// IMPORTANT — scope of this test: this checks the declared
/// <see cref="AuthorizeAttribute"/> metadata via reflection only. It does
/// NOT start the application, does NOT send an HTTP request, and does NOT
/// prove that an unauthorized caller is actually rejected at runtime —
/// that would require an HTTP-level integration test (e.g.
/// <c>WebApplicationFactory</c>), which this solution does not currently
/// have (see the Phase 9 report's "Integration test limitations" section).
/// This test only guards against someone removing the attribute by
/// accident; it is not a substitute for an end-to-end authorization test.
/// </remarks>
public class PermissionCatalogControllerAuthorizationTests
{
    [Fact]
    public void Controller_DeclaresAdminOnlyAuthorization()
    {
        var attribute = typeof(PermissionCatalogController)
            .GetCustomAttribute<AuthorizeAttribute>();

        Assert.NotNull(attribute);
        Assert.Equal("Admin", attribute!.Roles);
    }

    [Fact]
    public void SynchronizeAction_HasNoAdditionalAttributeThatWeakensControllerLevelAuthorization()
    {
        var method = typeof(PermissionCatalogController)
            .GetMethod(nameof(PermissionCatalogController.Synchronize));

        Assert.NotNull(method);

        // No [AllowAnonymous] must exist on the action — that would bypass
        // the controller-level [Authorize(Roles = "Admin")].
        var allowAnonymous = method!.GetCustomAttribute<AllowAnonymousAttribute>();
        Assert.Null(allowAnonymous);
    }
}
