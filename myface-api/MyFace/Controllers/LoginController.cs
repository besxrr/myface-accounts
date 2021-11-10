using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFace.Repositories;
using static MyFace.Helpers.PasswordHashHelper;
using static MyFace.Helpers.AuthorizationHelper;

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
            if (IsUserAuthorized(_users, Request))
            {
                return StatusCode(200);
            }
            return StatusCode(401, "Authentication Failed!");
        }
        
    }
    
}