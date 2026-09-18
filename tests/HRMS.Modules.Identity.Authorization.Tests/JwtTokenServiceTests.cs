using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HRMS.Modules.Identity.Application.Abstractions.Security;
using HRMS.Modules.Identity.Infrastructure.Security;
using Microsoft.Extensions.Options;
using Xunit;

namespace HRMS.Identity.Authorization.Tests;

/// <summary>
/// Verifies the token issuance behavior that the Phase 9 security review
/// depends on: the token carries role names only, no permission claims,
/// and roles are baked in at issuance time — which is the source of the
/// documented JWT role-staleness risk (a role change made after a token
/// is issued has no effect on that token until it expires and a new one
/// is generated).
/// </summary>
public class JwtTokenServiceTests
{
    private static JwtTokenService CreateService()
        => new(Options.Create(new JwtOptions
        {
            SecretKey = "this-is-a-test-only-secret-key-that-is-long-enough",
            Issuer = "HRMS.Tests",
            Audience = "HRMS.Tests.Client",
            ExpirationMinutes = 60
        }));

    private static JwtSecurityToken Decode(string token)
        => new JwtSecurityTokenHandler().ReadJwtToken(token);

    [Fact]
    public void GenerateToken_IncludesRoleClaimsForEveryRolePassedIn()
    {
        var service = CreateService();

        var token = service.GenerateToken(
            userId: Guid.NewGuid(),
            employeeId: Guid.NewGuid(),
            userName: "jane.doe",
            roles: new[] { "HR", "Manager" });

        var jwt = Decode(token);

        var roleClaims = jwt.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        Assert.Equal(new[] { "HR", "Manager" }, roleClaims);
    }

    [Fact]
    public void GenerateToken_NeverIncludesAPermissionClaim()
    {
        // Frozen-architecture guarantee: permissions are never encoded into
        // the JWT. Authorization is always re-evaluated against the
        // database by PermissionAuthorizationHandler.
        var service = CreateService();

        var token = service.GenerateToken(
            userId: Guid.NewGuid(),
            employeeId: Guid.NewGuid(),
            userName: "jane.doe",
            roles: new[] { "Admin" });

        var jwt = Decode(token);

        Assert.DoesNotContain(
            jwt.Claims,
            c => c.Type.Contains("permission", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void GenerateToken_WithNoRoles_ProducesNoRoleClaims()
    {
        var service = CreateService();

        var token = service.GenerateToken(
            userId: Guid.NewGuid(),
            employeeId: Guid.NewGuid(),
            userName: "jane.doe",
            roles: Array.Empty<string>());

        var jwt = Decode(token);

        Assert.DoesNotContain(
            jwt.Claims,
            c => c.Type == ClaimTypes.Role);
    }

    [Fact]
    public void GenerateToken_TwoTokensForTheSameUserWithDifferentRoles_CarryDifferentRoleClaims()
    {
        // Documents the staleness mechanism directly: role claims are a
        // snapshot taken at issuance. Nothing in the token itself is
        // re-checked against the database, so an already-issued token
        // keeps its original roles until it expires.
        var service = CreateService();
        var userId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();

        var tokenBeforeRoleChange = Decode(service.GenerateToken(
            userId, employeeId, "jane.doe", new[] { "Employee" }));

        var tokenAfterRoleChange = Decode(service.GenerateToken(
            userId, employeeId, "jane.doe", new[] { "Employee", "Manager" }));

        var rolesBefore = tokenBeforeRoleChange.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value);
        var rolesAfter = tokenAfterRoleChange.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value);

        Assert.DoesNotContain("Manager", rolesBefore);
        Assert.Contains("Manager", rolesAfter);
    }
}
