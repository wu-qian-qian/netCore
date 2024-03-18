using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Forum.JWT
{
    public class TokenService : ITokenService
    {
        public string BuildJwtString(IEnumerable<Claim> claims, JWTOptions options)
        {
            TimeSpan ts = TimeSpan.FromSeconds(options.ExpireSeconds);
            //构建密钥
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key));
            //加密算法进行密钥配置
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            //构建安全JWT
            var tokenDescriptor = new JwtSecurityToken(options.Issuer, options.Audience, claims, expires: DateTime.Now.Add(ts), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
