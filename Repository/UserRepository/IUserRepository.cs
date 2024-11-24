using EMS_Project.Models;

namespace EMS_Project.Repository.UserRepository
{
    public interface IUserRepository
    {
        Task<User> GetByEmail(string email);

        Task<User> GetByUsername(string username);

        Task AddUser(RegistedUser registedUser,string HashPW);
    }
}
