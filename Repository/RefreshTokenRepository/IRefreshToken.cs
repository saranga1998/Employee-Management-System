using EMS_Project.Models;

namespace EMS_Project.Repository.RefreshTokenRepository
{
    public interface IRefreshToken
    {
        Task AddRefreshToken(string refreshToken, string Id);

        Task<RefreshToken?> GetByToken(string Rtoken);

        Task DeleteToken(string TokenId);

        Task DeleteAllToken(string id);
    }
}
