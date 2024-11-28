using EMS_Project.Models;
using System.Security.Claims;
using EMS_Project.ViewModels.Requests;


namespace EMS_Project.Repository.TokenGenerator
{
    public class AccessTokenGenerator
    {
        //Injecting Resources
        private readonly AuthenticationConfiguration _configuration;
        private readonly TokenGenerator _tokenGenerator;

        public AccessTokenGenerator(AuthenticationConfiguration configuration, TokenGenerator tokenGenerator)
        {
            this._configuration = configuration;
            _tokenGenerator = tokenGenerator;
        }

        //Generate Access Token
        public string CreateAccessToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("id",user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.Username)
            };

            //Passing Values to Token Genreator
            return _tokenGenerator.CreateToken(
                _configuration.AccessTokenSecret,
                _configuration.Issuer,
                _configuration.Audience,
                _configuration.AccessTokenExpirationMinutes,
                claims
                );

        }
    }
}
