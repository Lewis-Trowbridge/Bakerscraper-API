using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Bakerscraper.Models;
using Bakerscraper.Factories;

namespace Bakerscraper.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {

        private readonly IRecipeSearchFactory searchFactory;
        private readonly IHttpClientFactory clientFactory;

        public RecipeController(IRecipeSearchFactory searchFactory, IHttpClientFactory clientFactory)
        {
            this.searchFactory = searchFactory;
            this.clientFactory = clientFactory;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("search")]
        public async Task<IEnumerable<Recipe>> Search([FromQuery]RecipeSearch searchRequest)
        {
            var searcher = searchFactory.CreateSearch(searchRequest.Type, clientFactory);
            return await searcher.Search(searchRequest.String, searchRequest.Limit);
        }
    }
}
