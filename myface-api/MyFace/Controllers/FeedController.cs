﻿using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using static MyFace.Helpers.AuthenticationHelper;

namespace MyFace.Controllers
{
    [Route("feed")]
    public class FeedController: ControllerBase
    {
        private readonly IPostsRepo _posts;
        private readonly IUsersRepo _users;

        public FeedController(IPostsRepo posts, IUsersRepo users)
        {
            _posts = posts;
            _users = users;
        }
        
        [HttpGet("")]
        public ActionResult<FeedModel> GetFeed([FromQuery] FeedSearchRequest searchRequest)
        {
            if (!IsUserAuthenticated(_users, Request))
            {
                return StatusCode(401, "Authentication Failed!");
            }

            var posts = _posts.SearchFeed(searchRequest);
            var postCount = _posts.Count(searchRequest);
            return FeedModel.Create(searchRequest, posts, postCount);
        }
        
        // [HttpGet("")]
        // public ActionResult<FeedModel> GetFeed([FromQuery] FeedSearchRequest searchRequest)
        // {
        //     if (!IsUserAuthenticated(_users, Request))
        //     {
        //         return StatusCode(401, "Authentication Failed!");
        //     }
        //
        //     var posts = _posts.SearchFeed(searchRequest);
        //     var postCount = _posts.Count(searchRequest);
        //     return FeedModel.Create(searchRequest, posts, postCount);
        // }
    }
}