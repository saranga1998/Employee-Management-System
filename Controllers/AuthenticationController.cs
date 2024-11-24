using EMS_Project.Models;
using EMS_Project.Repository.PasswordHasherRepository;
using EMS_Project.Repository.UserRepository;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Project.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHash _passwordHash;
        public AuthenticationController(IUserRepository userRepository,IPasswordHash passwordHash)
        {
            _userRepository = userRepository;
            _passwordHash = passwordHash;
        }


        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegistedUser registedUser)
        {
            if (ModelState.IsValid) 
            { 
                return BadRequest();
            }

            if (registedUser.Password != registedUser.ConfirmPassword)
            {
                return BadRequest();
            }

            User ExisitingUserByName = await _userRepository.GetByUsername(registedUser.Username);
            if (ExisitingUserByName != null)
            {
                return Conflict();
            }

            string Passwordhash =  _passwordHash.Hash(registedUser.Password);

            return View();
        }
    }
}
