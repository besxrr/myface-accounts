using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFace.Repositories;
using static MyFace.Helpers.PasswordHashHelper;

namespace MyFace.Helpers
{
    public class AuthenticationHelper
    {
        public static bool IsUserAuthenticated(IUsersRepo users, HttpRequest request)
        {
            return GetUserIdFromRequest(users, request) != null;
        }
        
        public static int? GetUserIdFromRequest(IUsersRepo users, HttpRequest request)
        {
            try
            {
                var authUser = DecodeAuthHeader(request.Headers["Authorization"]);
                var queryUser = users.GetUserByUsername(authUser.Username);
                var passwordHashed = GetHashedPassword(authUser.Password, queryUser.Salt);
                if (passwordHashed == queryUser.HashedPassword)
                {
                    return queryUser.Id;
                }
                //If password is wrong
                return null;
            }
            catch
            {
                //If user not in database
                return null;
            }
        }
    }

}