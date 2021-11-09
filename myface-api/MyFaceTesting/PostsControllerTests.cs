using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using MyFace;
using MyFace.Models.Database;
using MyFaceTesting.Helpers;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace MyFaceTesting
{
    public class PostsControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public PostsControllerTests(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task CheckGetsAResponse()
        {
            var response = await _client.GetAsync("/");
            response.EnsureSuccessStatusCode();
        }
        

        [Fact]
        public async Task CanGetPost()
        {
            var httpResponse = await _client.GetAsync("/posts/1");
            
            //Must be successful
            httpResponse.EnsureSuccessStatusCode();
            
            // Deserialize and examine results
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var post = JsonConvert.DeserializeObject<Post>(stringResponse);
            Assert.True(post.Message == "testMessage");
        }
        
        [Fact]
        public async Task CanCreatePostIfUserAuthorized()
        {
            var request = new
            {
                Url = "/posts/create",
                Body = new
                {
                    Message = "Integration test message",
                    ImageUrl = "Integration test image url",
                    UserId = 1
                }
            };
        
            _client.DefaultRequestHeaders.Add("Authorization", "Basic a3BsYWNpZG8wOmhlbGxv");
        
            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            //Must be successful
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var createdPost = JsonConvert.DeserializeObject<Post>(stringResponse);
            Assert.True(request.Body.Message == createdPost.Message);
        }
        
        [Fact]
        public async Task CannotCreatePostIfUserNotAuthorized()
        {
            var request = new
            {
                Url = "/posts/create",
                Body = new
                {
                    Message = "Integration test message",
                    ImageUrl = "Integration test image url",
                    UserId = 1
                }
            };
            
            //Change one letter in header
            _client.DefaultRequestHeaders.Add("Authorization", "Basic a3BsYWNpZG8wOmhlbGxa");
        
            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            //Must not be successful
            Assert.False(response.IsSuccessStatusCode);
        }
    }
}