using Azure.Core;
using EMS_Project.Models;
using EMS_Project.Repository.Authenticators;
using EMS_Project.Repository.PasswordHasherRepository;
using EMS_Project.Repository.RefreshTokenRepository;
using EMS_Project.Repository.TokenGenerator;
using EMS_Project.Repository.TokenValidator;
using EMS_Project.Repository.UserRepository;
using EMS_Project.ViewModels.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EMS_Project.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHash _passwordHash;
        private readonly IRefreshToken _refreshToken;
        private readonly Authenticator _authenticator;
        private readonly RefreshTokenValidator _refreshTokenValidator;

        public AuthenticationController(IUserRepository userRepository, IPasswordHash passwordHash, IRefreshToken refreshToken, Authenticator authenticator, RefreshTokenValidator refreshTokenValidator)
        {
            _userRepository = userRepository;
            _passwordHash = passwordHash;
            _refreshToken = refreshToken;
            _authenticator = authenticator;
            _refreshTokenValidator = refreshTokenValidator;
        }


        //User Registration
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RequsetViewModel request)
        {
            if (request == null)
            {
                return BadRequest(new ErrorViewModel("Invalid user data"));
            }

            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
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

        //User Login
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequset login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            bool existingUserByName = await _userRepository.GetByUsername(login.Username);
            if (!existingUserByName)
            {
                return Unauthorized(new ErrorViewModel(errorMessage: "Username is not exists"));
            }

            User? user = await _userRepository.GetUserDetails(login.Username);
            if (user == null)
            {
                return Unauthorized(new ErrorViewModel(errorMessage: "User details could not be retrieved"));
            }

            bool isCorrectPassword = _passwordHash.VerifyPassword(login.Password, user.PasswordHash);

            if (!isCorrectPassword)
            {
                return Unauthorized(new ErrorViewModel(errorMessage: "Invalid password"));
            }

            AuthenticatedUserResponse response = await _authenticator.Authenticate(user);
            return Ok(response);
        }

        //Refresh
        [HttpPost]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refresh)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            bool IsValidateToken = _refreshTokenValidator.Validate(refresh.RefreshToken);
            if (!IsValidateToken)
            {
                return BadRequest(new ErrorViewModel(errorMessage: "Invalid Refresh Token"));
            }

            RefreshToken refreshToken = await _refreshToken.GetByToken(refresh.RefreshToken);
            if (refreshToken == null)
            {
                return NotFound(new ErrorViewModel(errorMessage: "Refresh Token not found"));
            }

            await _refreshToken.DeleteToken(refreshToken.TokenId);

            User user = await _userRepository.GetbyUserId(refreshToken.Id);

            if (user == null)
            {
                return NotFound(new ErrorViewModel(errorMessage: "User Not Found"));
            }


            AuthenticatedUserResponse response = await _authenticator.Authenticate(user);
            return Ok(response);
        }


        //Logout
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Logout()
        {
            string RawUserId = HttpContext.User.FindFirstValue("id");

            //if(!Guid.TryParse(RawUserId,out Guid userID))
            //{
            //    return Unauthorized();
            //}
            await _refreshToken.DeleteAllToken(RawUserId);

            return NoContent();

        }


        //Bad Request Method
        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values
                    .SelectMany(x => x.Errors.Select(e => e.ErrorMessage));
            return BadRequest(errorMessages);
        }
    }
}
