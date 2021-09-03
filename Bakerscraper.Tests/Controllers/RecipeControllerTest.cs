using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public async void RecipeController_WhenCalledWithBBCGoodFood_CallsConstructor()
        {
            var factoryMock = new Mock<IRecipeSearchFactory>();
            var searcherMock = new Mock<BBCGoodFoodRecipeSearch>();
            searcherMock.Setup(mock => mock.Search(It.IsAny<string>()).Result).Returns(new List<Recipe>());
            factoryMock.Setup(mock => mock.CreateSearch(RecipeSearchType.BBCGoodFood)).Returns(searcherMock.Object);

            var testRequest = new RecipeSearch { Type = RecipeSearchType.BBCGoodFood };

            var controller = new RecipeController(factoryMock.Object);

            await controller.Search(testRequest);

            factoryMock.Verify(mock => mock.CreateSearch(RecipeSearchType.BBCGoodFood));
            
        }
    }
}
