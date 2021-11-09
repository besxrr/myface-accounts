using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using MyFace;
using Xunit;
using Xunit.Abstractions;

namespace MyFaceTesting
{
    public class LoginControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public LoginControllerTests(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
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


    }
}