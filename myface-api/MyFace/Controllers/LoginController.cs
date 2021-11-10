using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Database;
using MyFace.Repositories;
using static MyFace.Helpers.PasswordHashHelper;
using static MyFace.Helpers.AuthenticationHelper;
using MyFace.Repositories;

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
            if (IsUserAuthenticated(_users, Request))
            {
                return StatusCode(200);
            }
            return StatusCode(401, "Authentication Failed!");
        }
    }
}