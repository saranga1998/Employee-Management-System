using EMS_Project.ViewModels.Requests;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Text;


namespace EMS_Project.Repository.TokenValidator
{
    public class RefreshTokenValidator
    {
        //Injecting Resources
        private readonly AuthenticationConfiguration _configuration;

        public RefreshTokenValidator(AuthenticationConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Validate(string refreshToken)
        {
            TokenValidationParameters parameters = new TokenValidationParameters()
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration.Issuer,
                ValidAudience = _configuration.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.RefreshTokenSecret))
            };


            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(refreshToken, parameters, out SecurityToken securityToken);
                return true;
            }
            catch (Exception ex)
            {
                {
                    return false;
                }

            }

        }

    }
}
