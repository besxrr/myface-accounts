using System;
using Microsoft.AspNetCore.Mvc;
using MyFace.Repositories;
using static MyFace.Helpers.PasswordHashHelper;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("/login")]
    public class LoginController  : ControllerBase
    {
        private readonly IUsersRepo _users;

        public LoginController(IUsersRepo users)
        {
            _users = users;
        }

        [HttpGet("")]
        public IActionResult LogIn()
        {
            try
            {
                var authUser = DecodeAuthHeader(Request.Headers["Authorization"]);
                var queryUser = _users.QueryByUsername(authUser.Username);
                var passwordHashed = GetHashedPassword(authUser.Password, queryUser.Salt);
                if (passwordHashed == queryUser.HashedPassword)
                {
                    return StatusCode(200);
                }
                throw new Exception();
            }
            catch (Exception e)
            {
                return StatusCode(401, "Authentication Failed!");
            }
            
        }
    }
    
}