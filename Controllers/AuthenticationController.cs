using EMS_Project.Models;
using EMS_Project.Repository.PasswordHasherRepository;
using EMS_Project.Repository.UserRepository;
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
        public async Task<IActionResult> RegisterUser([FromBody]RegistedUser registedUser)
        {
            string Passwordhash = _passwordHash.Hash(registedUser.Password);
            if (ModelState.IsValid) 
            { 
                IEnumerable<string> errorMessages = ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage));
                return BadRequest(errorMessages);
            }
            

            //if (registedUser.Password != registedUser.ConfirmPassword)
            //{
            //    return BadRequest(new ErrorViewModel("Password Not matched"));
            //}
            
                
            //bool ExisitingUserByName = await _userRepository.GetByUsername(registedUser.Username);
            bool ExisitingUserByEmail = await _userRepository.GetByEmail(registedUser.Email);

                //if (ExisitingUserByName != true)
                //{
                //    return Conflict(new ErrorViewModel("Username is Exisit"));
                //}


                //if (ExisitingUserByEmail != true)
                //{
                //    return Conflict(new ErrorViewModel("Email is Exisit"));
                //}

                
                await _userRepository.AddUser(registedUser, Passwordhash);
                return Ok();
           

            
        }
    }
}
