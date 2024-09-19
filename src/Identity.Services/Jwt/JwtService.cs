using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Services.Jwt
{
    public class JwtService
    {
        private string secureKey = "aAdaZRzW9wjr8hIpSH3M6/I1IY2NtFir+r67pGDiBnY=";

        public string Generate(string customerInfo)
        {
            
            var claims = new[]
            {
                new Claim("CustomerInfo",customerInfo)
            };
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentials);

            var payload = new JwtPayload("null", "null", claims, null, DateTime.Today.AddDays(1)); // 1 day
            var securityToken = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public JwtSecurityToken Verify(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secureKey);
            tokenHandler.ValidateToken(jwt, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false
            }, out SecurityToken validatedToken);

            return (JwtSecurityToken)validatedToken;
        }
    }
}

 // using (var rng = new RNGCryptoServiceProvider())
 //        {
 //            var key = new byte[256 / 8];
 //            rng.GetBytes(key);
 //            var r= Convert.ToBase64String(key);
 //            Console.WriteLine(r);
 //        }
