using System.Security.Claims;

namespace Tet.Service.JwtService;

public interface IJwtService
{
    public String GenerateAccessToken(IEnumerable<Claim> claims);
    ClaimsPrincipal ValidateToken(String token);
}