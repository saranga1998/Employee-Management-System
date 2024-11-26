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
        public string CreateToken(User user)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.AccessTokenSecret));

            SigningCredentials credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>()
            {
                new Claim("id",user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.Username)
            };

            JwtSecurityToken token = new JwtSecurityToken(
                configuration.Issuer,
                configuration.Audience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(configuration.AccessTokenExpirationMinutes),
                credentials

                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
