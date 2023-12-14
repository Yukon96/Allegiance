using Microsoft.AspNetCore.Mvc;
using Capstone.DAO;
using Capstone.Exceptions;
using Capstone.Models;
using Capstone.Security;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ITokenGenerator tokenGenerator;
        private readonly IPasswordHasher passwordHasher;
        private readonly IUserDao userDao;

        public LoginController(ITokenGenerator tokenGenerator, IPasswordHasher passwordHasher, IUserDao userDao)
        {
            this.tokenGenerator = tokenGenerator;
            this.passwordHasher = passwordHasher;
            this.userDao = userDao;
        }

        [HttpGet("/")]
        public ActionResult<string> Ready()
        {
            int userCount = userDao.GetUsers().Count;
            return Ok($"Server is ready with {userCount} user(s).");
        }

        //GET /whoami
        [HttpGet("/whoami")]
        public ActionResult<string> WhoAmI()
        {
            string result = User.Identity.Name;
            if (result == null)
            {
                return "No token provided.";
            }
            else
            {
                return result;
            }
        }

        [HttpPost("/login")]
        public IActionResult Authenticate(LoginUser userParam)
        {
            // Default to bad username/password message
            IActionResult result = Unauthorized(new { message = "Username or password is incorrect." });

            User user;
            // Get the user by username
            try
            {
                user = userDao.GetFullUserByUsername(userParam.Username);
            }
            catch (DaoException)
            {
                // return default Unauthorized message instead of indicating a specific error
                return result;
            }

            // If we found a user and the password hash matches

            if (user != null && userParam.Password != null && passwordHasher.VerifyHashMatch(user.PasswordHash, userParam.Password, user.Salt))
            {
                // Create an authentication token
                string token = tokenGenerator.GenerateToken(user.UserId, user.Username, user.Role);

                // Create a ReturnUser object to return to the client
                LoginResponse retUser = new LoginResponse() { User = new ReturnUser() { UserId = user.UserId, Username = user.Username, Role = user.Role, HasOneTimePassword = user.HasOneTimePassword }, Token = token };

                if (user.HasOneTimePassword)
                {
                    result = Accepted(retUser);
                }

                // Switch to 200 OK
                else
                {
                    result = Ok(retUser);
                }
            }
            return result;
        }

        [HttpPost("/register")]
        public IActionResult Register(RegisterUser userParam)
        {
            int test = 7;
            // Default generic error message
            const string ErrorMessage = "An error occurred and user was not created.";

            IActionResult result = BadRequest(new { message = ErrorMessage });

            // is username already taken?
            try
            {
                User existingUser = userDao.GetFullUserByUsername(userParam.Username);
                if (existingUser != null)
                {
                    return Conflict(new { message = "Username already taken. Please choose a different username." });
                }
                if (userParam.ConfirmPassword != userParam.Password)
                {
                    return Conflict(new { message = "Passwords do not match, please verify that passwords match" });
                }
            }
            catch (DaoException)
            {
                return StatusCode(500, ErrorMessage);
            }

            // is email already taken?
            try
            {
                User existingUser = userDao.GetFullUserByEmail(userParam.Email);
                if (existingUser != null)
                {
                    return Conflict(new { message = "Email already taken. Please choose a different Email." });
                }
                if (userParam.ConfirmPassword != userParam.Password)
                {
                    return Conflict(new { message = "Passwords do not match, please verify that passwords match" });
                }
            }
            catch (DaoException)
            {
                return StatusCode(500, ErrorMessage);
            }
            // create new user
            User newUser;
            try
            {
                newUser = userDao.CreateUser(userParam.Username, userParam.Email, userParam.OrganizationName, userParam.Password, userParam.Role);
            }
            catch (DaoException)
            {
                return StatusCode(500, ErrorMessage);
            }

            if (newUser != null)
            {
                // Create a ReturnUser object to return to the client
                ReturnUser returnUser = new ReturnUser() { UserId = newUser.UserId, Username = newUser.Username, Role = newUser.Role };

                result = Created("/login", returnUser);
            }

            return result;
        }
    }
}
