using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Bakerscraper.Searchers;
using Bakerscraper.Factories;
using Bakerscraper.Models;
using Bakerscraper.Controllers;
using Bakerscraper.Enums;
using Xunit;
using Moq;

namespace Bakerscraper.Tests.Controllers
{
    public class RecipeControllerTest
    {
        [Fact]
        public async void RecipeController_WhenCalledWithBBCGoodFood_CallsFactoryWithEnum()
        {   
            // Mock a searcher to avoid using real searcher code in the controller
            var searcherMock = new Mock<IRecipeSearch>();
            searcherMock.Setup(mock => mock.Search(It.IsAny<string>()).Result).Returns(new List<Recipe>());
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var searchFactoryMock = new Mock<IRecipeSearchFactory>();
            searchFactoryMock.Setup(mock => mock.CreateSearch(RecipeSearchType.BBCGoodFood, httpClientFactoryMock.Object)).Returns(searcherMock.Object);

            var testRequest = new RecipeSearch { Type = RecipeSearchType.BBCGoodFood };

            var controller = new RecipeController(searchFactoryMock.Object, httpClientFactoryMock.Object);

            await controller.Search(testRequest);

            searchFactoryMock.Verify(mock => mock.CreateSearch(RecipeSearchType.BBCGoodFood, httpClientFactoryMock.Object));
            
        }
    }
}
