using EMS_Project.Models;
using EMS_Project.Repository.RefreshTokenRepository;
using EMS_Project.Repository.TokenGenerator;
using EMS_Project.ViewModels.Requests;

namespace EMS_Project.Repository.Authenticators
{
    
    public class Authenticator
    {
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly IRefreshToken _refreshToken;

        public Authenticator(AccessTokenGenerator accessTokenGenerator, RefreshTokenGenerator refreshTokenGenerator, IRefreshToken refreshToken)
        {
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshToken = refreshToken;
        }

        public async Task<AuthenticatedUserResponse> Authenticate(User user)
        {
            string accessToken = _accessTokenGenerator.CreateAccessToken(user);
            string refreshToken = _refreshTokenGenerator.CreateRefreshToken();

            await _refreshToken.AddRefreshToken(refreshToken, user.Id);

            return (new AuthenticatedUserResponse()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                }
            );
        }
    }
}
