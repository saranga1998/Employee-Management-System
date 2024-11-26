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
        public async Task<bool> GetByEmail(string email)
        {
            return await _appDbContext.Users.AnyAsync(e => e.Email == email);
        }

        public async Task<bool> GetByUsername(string username)
        {
            return await _appDbContext.Users.AnyAsync(e => e.Username == username);
        }
        public async Task AddUser(RequsetViewModel requset, string HashPW)
        {

            if (requset == null)
            {
                throw new ArgumentNullException(nameof(requset), "Registered user cannot be null");
            }

            try
            {
                var newUser = new User
                {
                    Id = Guid.NewGuid().ToString("N").Substring(0, 8), // Generate a new unique ID
                    Email = requset.Email,
                    Username = requset.Username,
                    PasswordHash = HashPW
                };

                await _appDbContext.Users.AddAsync(newUser);
                await _appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log and rethrow the exception
                throw new InvalidOperationException("An error occurred while adding a new user", ex);
            }
        }

        
    }
}
