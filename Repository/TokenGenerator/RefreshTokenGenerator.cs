using EMS_Project.ViewModels.Requests;
using System.Security.Claims;

namespace EMS_Project.Repository.TokenGenerator
{
    public class RefreshTokenGenerator
    {
        //Injecting Resources
        private readonly AuthenticationConfiguration _configuration;
        private readonly TokenGenerator _tokenGenerator;

        public RefreshTokenGenerator(AuthenticationConfiguration configuration, TokenGenerator tokenGenerator)
        {
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
        }

        //Generate Refresh Token
        public string CreateRefreshToken() 
        {

            return _tokenGenerator.CreateToken(
                    _configuration.RefreshTokenSecret,
                    _configuration.Issuer,
                    _configuration.Audience,
                    _configuration.RefreshTokenExpirationMinutes
                    
                    );
        }
    }
}
