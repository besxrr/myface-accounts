using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFace.Repositories;
using static MyFace.Helpers.PasswordHashHelper;

namespace MyFace.Helpers
{
    public class AuthorizationHelper
    {
        public static bool IsUserAuthorized(IUsersRepo _users, HttpRequest request )
        {
            try
            {
                var authUser = DecodeAuthHeader(request.Headers["Authorization"]);
                var queryUser = _users.QueryByUsername(authUser.Username);
                var passwordHashed = GetHashedPassword(authUser.Password, queryUser.Salt);
                if (passwordHashed == queryUser.HashedPassword)
                {
                   return true;
                }
                //If password is wrong
                return false;
            }
            catch
            {
                //If user not in database
                return false;
            }
        }
    }

}