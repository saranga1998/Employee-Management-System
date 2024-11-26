namespace EMS_Project.Repository.PasswordHasherRepository
{
    public interface IPasswordHash
    {
        string Hash(string password);

        bool VerifyPassword(string password,string passwordHash);
    }
}
