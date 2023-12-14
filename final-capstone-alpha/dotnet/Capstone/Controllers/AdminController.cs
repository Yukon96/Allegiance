using Microsoft.AspNetCore.Mvc;
using Capstone.DAO;
using Capstone.Exceptions;
using Capstone.Models;
using Capstone.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Capstone.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserDao userDao;

        public AdminController(IUserDao userDao)
        {
            this.userDao = userDao;
        }

        [Authorize(Roles = "admin")]
        [HttpPut("resetpassword/{id}")]
        public IActionResult ResetPassword(int id)
        {
            User user = userDao.GetUserById(id);
            string oneTimePassword;
            try
            {
                oneTimePassword = userDao.GenerateOneTimePassword(user.Username);
            }
            catch (DaoException)
            {
                return StatusCode(500, "An internal server error occurred.");
            }
            return Created("admin/resetpassword", oneTimePassword);
        }
       
        [HttpGet("users")]
        public ActionResult<IList> GetUsers()
        {
            IList<ReturnUser> user_list = new List<ReturnUser>();

            foreach (ReturnUser user in userDao.GetUsers())
            {
                ReturnUser retUser = new ReturnUser()
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Role = user.Role,
                    Email = user.Email
                };
                
                user_list.Add(retUser);
            }

            return Ok(user_list);
        }
    }
}