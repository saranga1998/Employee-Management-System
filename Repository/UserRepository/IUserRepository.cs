using EMS_Project.Models;

namespace EMS_Project.Repository.UserRepository
{
    public interface IUserRepository
    {
        Task<bool> GetByEmail(string email);

        Task<bool> GetByUsername(string username);

        Task AddUser(RegistedUser registedUser,string HashPW);
    }
}
