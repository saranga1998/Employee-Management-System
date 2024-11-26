using EMS_Project.Data;
using EMS_Project.Models;
using EMS_Project.ViewModels.Requests;
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
        public async Task AddUser(RequsetViewModel requset, string HashPW)
        {
            var NewUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = requset.Email,
                Username = requset.Username,
                PasswordHash = HashPW
            };
            await _appDbContext.Users.AddAsync(NewUser);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<bool> GetByEmail(string email)
        {           
             return await _appDbContext.Users.AnyAsync(e => e.Email == email);    
        }

        public async Task<bool> GetByUsername(string username)
        {
            return await _appDbContext.Users.AnyAsync(e => e.Username == username);   
        }
    }
}
