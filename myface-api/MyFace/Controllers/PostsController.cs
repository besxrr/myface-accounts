﻿using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MyFace.Helpers;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using static MyFace.Helpers.PasswordHashHelper;
using static MyFace.Helpers.AuthenticationHelper;


namespace MyFace.Controllers
{
    [ApiController]
    [Route("/posts")]
    public class PostsController : ControllerBase
    {    
        private readonly IPostsRepo _posts;

        private readonly IUsersRepo _users;

        public PostsController(IPostsRepo posts, IUsersRepo users)
        {
            _posts = posts;
            _users = users;
        }

        [HttpGet("")]
        public ActionResult<PostListResponse> Search([FromQuery] PostSearchRequest searchRequest)
        {
            var posts = _posts.Search(searchRequest);
            var postCount = _posts.Count(searchRequest);
            return PostListResponse.Create(searchRequest, posts, postCount);
        }

        [HttpGet("{id}")]
        public ActionResult<PostResponse> GetById([FromRoute] int id)
        {
            var post = _posts.GetById(id);
            return new PostResponse(post);
        }
        
        [HttpGet("{id}/interactions")]
        public GetInteractionCountResponse GetInteractionCountByPostId([FromRoute] int id)
        {
            var interactionCount = _posts.GetInteractionCountByPostId(id);
            
            return new GetInteractionCountResponse(interactionCount);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreatePostRequest newPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetUserIdFromRequest(_users, Request);
            if (userId == null)
            {
                return StatusCode(401, "Authentication Failed!");
            } 
            
            newPost.UserId = (int) userId;
            var post = _posts.Create(newPost);
            var url = Url.Action("GetById", new {id = post.Id});
            var postResponse = new PostResponse(post);
            return Created(url, postResponse);
        }

        [HttpPatch("{id}/update")]
        public ActionResult<PostResponse> Update([FromRoute] int id, [FromBody] UpdatePostRequest update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var post = _posts.Update(id, update);
            return new PostResponse(post);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            _posts.Delete(id);
            return Ok();
        }
    }
}