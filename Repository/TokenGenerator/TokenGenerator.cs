using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EMS_Project.Repository.TokenGenerator
{
    public class TokenGenerator
    {
        public string CreateToken()
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(""));

            SigningCredentials credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>()
            {

            };

            JwtSecurityToken token = new JwtSecurityToken();

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
