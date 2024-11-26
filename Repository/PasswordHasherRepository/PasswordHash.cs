using BCrypt.Net;

namespace EMS_Project.Repository.PasswordHasherRepository
{
    public class PasswordHash : IPasswordHash
    {
        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);    
        }
    }
}
