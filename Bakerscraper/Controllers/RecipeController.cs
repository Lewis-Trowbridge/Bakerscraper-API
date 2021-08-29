using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("search")]
        public Recipe Search([FromQuery]RecipeSearch searchRequest)
        {
            var factory = new RecipeSearchFactory();
            var searcher = factory.CreateSearch(searchRequest.Type);
            return searcher.Search(searchRequest.String);
        }
    }
}
