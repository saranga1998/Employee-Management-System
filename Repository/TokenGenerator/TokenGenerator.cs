using EMS_Project.Models;
using EMS_Project.ViewModels.Requests;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EMS_Project.Repository.TokenGenerator
{
    public class TokenGenerator
    {
        private readonly AuthenticationConfiguration configuration;

        public TokenGenerator(AuthenticationConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string CreateToken(string SecrutKey,string issuer,string audience,int ExpMin,IEnumerable<Claim> claims = null)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecrutKey));

            SigningCredentials credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            

            JwtSecurityToken token = new JwtSecurityToken(
                configuration.Issuer,
                configuration.Audience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(ExpMin),
                credentials

                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
