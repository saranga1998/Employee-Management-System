using EMS_Project.Models;
using EMS_Project.ViewModels.Requests;

namespace EMS_Project.Repository.UserRepository
{
    public interface IUserRepository
    {
        Task<bool> GetByEmail(string email);

        Task<bool> GetByUsername(string username);

        Task<User?> GetUserDetails(string username);

        Task<User?> GetbyUserId(string userId);

        Task AddUser(RequsetViewModel requset,string HashPW);
    }
}
