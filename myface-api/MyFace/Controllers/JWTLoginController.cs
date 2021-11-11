using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Response;
using MyFace.Repositories;
using static MyFace.Helpers.AuthenticationHelper;
using static MyFace.Helpers.JWTAuthHelper;


namespace MyFace.Controllers
{
    
    [ApiController]
    [Route("/JWTLogin")]
    public class JWTLoginController  : ControllerBase
    {
        private readonly IUsersRepo _users;

        public JWTLoginController(IUsersRepo users)
        {
            _users = users;
        }

        [HttpGet("")] public ActionResult<JWTLoginResponse> JWTlogIn()
        {
            if (IsUserAuthenticated(_users, Request))
            {
                var userId = GetUserIdFromRequest(_users, Request);
                var user = _users.GetById((int) userId);
                var token = GenerateToken(user.Id, user.Role);
                return new JWTLoginResponse(token);
            }

            return StatusCode(401, "Authentication Failed!");
        }
    }
}