using EMS_Project.Models;
using EMS_Project.Repository.UserRepository;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Project.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        [HttpPost]
        public IActionResult RegisterUser(RegistedUser registedUser)
        {
            if (ModelState.IsValid) 
            { 
                return BadRequest();
            }

            if (registedUser.Password != registedUser.ConfirmPassword)
            {
                return BadRequest();
            }

            return View();
        }
    }
}
