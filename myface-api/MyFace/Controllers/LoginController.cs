using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Database;
using MyFace.Models.Response;
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
        public GetLoginResponse LogIn()
        {
            if (IsUserAuthenticated(_users, Request))
            {
                var userRole = GetUserIdFromRequest(_users, Request);
                var user = _users.GetById((int) userRole);
                return new GetLoginResponse(user.Role);
            }
            return null;
        }
    }
}