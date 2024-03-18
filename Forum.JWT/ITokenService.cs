using System.Security.Claims;

namespace Forum.JWT
{
    public interface ITokenService
    {
        string BuildJwtString(IEnumerable<Claim> claims, JWTOptions options);
    }
}