namespace HRMS.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken( Guid userId, Guid employeeId,string userName);
}