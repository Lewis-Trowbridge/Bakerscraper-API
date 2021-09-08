using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Bakerscraper.Tests.Functional_Tests
{
    public class BSFunctionalTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public BSFunctionalTest()
        {
            _factory = new WebApplicationFactory<Startup>();
        }

        [Fact]
        public async void Get_SwaggerPage_ReturnsSuccessCode()
        {
            var expectedResposeCode = HttpStatusCode.OK;

            var client = _factory.CreateClient();

            var response = await client.GetAsync("/swagger");

            Assert.Equal(expectedResposeCode, response.StatusCode);

        }
        
        [Fact]
        public async void Get_SwaggerPage_ReturnsHTML()
        {
            var expectedResponseType = "text/html";

            var client = _factory.CreateClient();

            var response = await client.GetAsync("/swagger");

            Assert.Equal(expectedResponseType, response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async void Get_RecipeSearch_ReturnsCorrectJSON()
        {
            var expectedResponse = File.ReadAllText("Functional Tests/Assets/ksearchjson.json");
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/recipe/search?string=k");

            Assert.Equal(expectedResponse, await response.Content.ReadAsStringAsync());
        }

    }
}
