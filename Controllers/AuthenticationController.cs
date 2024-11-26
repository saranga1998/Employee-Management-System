using EMS_Project.Models;
using EMS_Project.Repository.PasswordHasherRepository;
using EMS_Project.Repository.UserRepository;
using EMS_Project.ViewModels.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<IActionResult> RegisterUser(RequsetViewModel request)
        {
            if (request == null)
            {
                return BadRequest(new ErrorViewModel("Invalid user data"));
            }

            if (!ModelState.IsValid)
            {
                IEnumerable<string> errorMessages = ModelState.Values
                    .SelectMany(x => x.Errors.Select(e => e.ErrorMessage));
                return BadRequest(errorMessages);
            }


            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest(new ErrorViewModel("Passwords do not match"));
            }
            bool existingUserByName = await _userRepository.GetByUsername(request.Username);
            if (existingUserByName)
            {
                return Conflict(new ErrorViewModel("Username already exists"));
            }

            bool existingUserByEmail = await _userRepository.GetByEmail(request.Email);
            if (existingUserByEmail)
            {
                return Conflict(new ErrorViewModel("Email already exists"));
            }

            string Passwordhash = _passwordHash.Hash(request.Password);
            await _userRepository.AddUser(request, Passwordhash);
            return Ok("Registration successful");
               
        }
    }
}
