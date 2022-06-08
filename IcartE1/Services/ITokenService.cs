using System.Collections.Generic;
using System.Security.Claims;

namespace IcartE1.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> authClaims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);
    }
}