using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            var client = _factory.CreateClient();

            var expectedResponse = File.ReadAllText("Functional Tests/Assets/ksearchjson.json");
            var jExpectedResponse = JArray.Parse(expectedResponse);

            var response = await client.GetAsync("/api/recipe/search?string=k");
            var jResponse = JArray.Parse(await response.Content.ReadAsStringAsync());

            jResponse.Should().BeEquivalentTo(jExpectedResponse);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public async void Get_RecipeSearchWithLimit_ReturnsSizedList(int size)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync($"/api/recipe/search?string=cake&type=bbcgoodfood&limit={size}");

            var jsonResponse = JArray.Parse(await response.Content.ReadAsStringAsync());

            jsonResponse.Should().HaveCount(size);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public async void Get_RecipeSearchWithLimitOutsideOfRange_Returns400(int size)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync($"/api/recipe/search?string=cake&type=bbcgoodfood&limit={size}");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

    }
}
