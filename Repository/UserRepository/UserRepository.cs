using EMS_Project.Data;
using EMS_Project.Models;

namespace EMS_Project.Repository.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserRepository(AppDbContext appDbContext) 
        { 
            _appDbContext = appDbContext;
        }
        public async Task AddUser(RegistedUser registedUser, string HashPW)
        {
            var NewUser = new User
            {
                Email = registedUser.Email,
                Username = registedUser.Username,
                PasswordHash = HashPW
            };
            await _appDbContext.Users.AddAsync(NewUser);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<User> GetByEmail(string email)
        {
            return await Task.FromResult(await _appDbContext.Users.FindAsync(keyValues: email));
        }

        public async Task<User> GetByUsername(string username)
        {
            return await Task.FromResult(await _appDbContext.Users.FindAsync(keyValues: username));
        }
    }
}
