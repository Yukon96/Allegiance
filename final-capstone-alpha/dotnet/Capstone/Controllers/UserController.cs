using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Capstone.DAO;
using Capstone.Exceptions;
using Capstone.Models;
using Capstone.Security;
using Microsoft.AspNetCore.Authorization;
using Capstone.Security.Models;

namespace Capstone.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserDao userDao;

        public UserController(IUserDao userDao)
        {
            this.userDao = userDao;
        }

        [HttpPut("changepassword")]
        public IActionResult ChangePassword(RecoverUser user)
        {
            if (user.Password == "")
            {
                return StatusCode(400, "Password may not be empty.");
            }
            if (user.Password != user.ConfirmPassword)
            {
                return StatusCode(400, "Passwords do not match.");
            }
            try
            {
                userDao.ChangePassword(user.Username, user.ConfirmPassword);
            }
            catch (DaoException)
            {
                return StatusCode(500, "An internal server error occurred.");
            }
            return Ok();
        }
    }
}

