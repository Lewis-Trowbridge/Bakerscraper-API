﻿using Microsoft.AspNetCore.Http;
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

        private readonly IRecipeSearchFactory searchFactory;

        public RecipeController(IRecipeSearchFactory searchFactory)
        {
            this.searchFactory = searchFactory;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("search")]
        public async Task<List<Recipe>> Search([FromQuery]RecipeSearch searchRequest)
        {
            var searcher = searchFactory.CreateSearch(searchRequest.Type);
            return await searcher.Search(searchRequest.String);
        }
    }
}
