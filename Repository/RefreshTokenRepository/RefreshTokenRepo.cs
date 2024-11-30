using EMS_Project.Models;
using Microsoft.EntityFrameworkCore;
using EMS_Project.Data;
namespace EMS_Project.Repository.RefreshTokenRepository
{
    public class RefreshTokenRepo : IRefreshToken
    {
        private readonly AppDbContext _appDbContext;
        private readonly List<RefreshToken> _refreshTokens = new List<RefreshToken>();
        public RefreshTokenRepo(AppDbContext appDbContext) {
            _appDbContext = appDbContext;
        }

        //Add Refresh token
        public async Task AddRefreshToken(string refreshToken, string Id)
        {
            try
            {
                var NewRefreshToken = new RefreshToken()
                {
                    TokenId = Guid.NewGuid().ToString("N").Substring(0, 8), // Generate a new unique ID
                    Token = refreshToken,
                    Id = Id,
                };

                await _appDbContext.RefreshTokens.AddAsync(NewRefreshToken);
                await _appDbContext.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                // Log and rethrow the exception
                throw new InvalidOperationException("An error occurred while adding a Refresh token", ex);
            }
        }

        public async Task DeleteAllToken(string id)
        {
            // Fetch all tokens for the given user ID
            var tokens = await _appDbContext.RefreshTokens
                                            .Where(t => t.Id == id)
                                            .ToListAsync();

            // Check if there are tokens to delete
            if (tokens.Any())
            {
                _appDbContext.RefreshTokens.RemoveRange(tokens);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteToken(string TokenId)
        {
            var refreshToken = await _appDbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.TokenId == TokenId);

            if (refreshToken == null)
            {
                throw new ArgumentException("Token not found", nameof(TokenId));
            }

            // Remove the entity
            _appDbContext.RefreshTokens.Remove(refreshToken);

            // Save changes to the database
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetByToken(string Rtoken)
        {
            RefreshToken refreshToken = await _appDbContext.RefreshTokens.FirstOrDefaultAsync(rt => EF.Functions.Like(rt.Token, Rtoken));

            return refreshToken;
        }
    }
}
