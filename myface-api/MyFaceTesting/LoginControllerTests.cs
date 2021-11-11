using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using MyFace;
using MyFace.Models.Database;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace MyFaceTesting
{
    public class LoginControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;

        public LoginControllerTests(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task AuthorizedUserCanLogin()
        {
            _client.DefaultRequestHeaders.Add("Authorization", "Basic a3BsYWNpZG8wOmhlbGxv");
            var httpResponse = await _client.GetAsync("/login");
            httpResponse.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task UnAuthorizedUserCannotLogin()
        {
            var httpResponse = await _client.GetAsync("/login");
            Assert.True(httpResponse.StatusCode == HttpStatusCode.Unauthorized);
            Assert.True(httpResponse.ReasonPhrase == "Unauthorized");
        }

        [Fact]
        public async Task AuthorizedUserCanGenerateJWT()
        {
            _client.DefaultRequestHeaders.Add("Authorization", "Basic a3BsYWNpZG8wOmhlbGxv");
            var httpResponse = await _client.GetAsync("/JWTlogin");
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Contains("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiw", stringResponse);
        }
    }
}