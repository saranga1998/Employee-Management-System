using EMS_Project.Data;
using EMS_Project.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> GetByEmail(string email)
        {           
            var IsemailExisit = await _appDbContext.Users.AnyAsync(e => e.Email == email);

            return IsemailExisit;
        }

        public async Task<bool> GetByUsername(string username)
        {
            bool IsUsernameExisit = true;
            var Username = await _appDbContext.Users.FindAsync(username);
            if (Username.Username != username)
            {
                IsUsernameExisit = true;
            }
            else
            {
                IsUsernameExisit = false;
            }
            return IsUsernameExisit;
            
        }
    }
}
