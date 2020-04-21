using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pokemonapi.Models;
using pokemonapi.Services.Interfaces;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.Results;

namespace pokemonapi.Controllers
{
    [ApiController]
    [System.Web.Http.Route("pokemon")]
    public class PokemonController : ControllerBase
    {
        private readonly ILogger<PokemonController> _logger;
        private readonly IPokemonService _pokemonService;

        public PokemonController(ILogger<PokemonController> logger, IPokemonService pokemonService)
        {
            _logger = logger;
            _pokemonService = pokemonService;
        }

        [SwaggerOperation("Retrieve Shakespearean Pokemon Description")]
        [SwaggerResponse(HttpStatusCode.OK, "Retrieve Shakespearean Pokemon Description", typeof(PokemonResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Received invalid input parameters", typeof(ModelState))]
        [Microsoft.AspNetCore.Mvc.HttpGet("pokemon/{pokemonName}")]
        public async Task<IActionResult> RetrieveShakespeareanDescription(string pokemonName)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(pokemonName))
            {
                return BadRequest(ModelState);
            }

            PokemonResponse response = await _pokemonService.RetrieveShakespeareanDescription(pokemonName);

            if (response.Success)
            {
                return Ok(response.Response);
            }

            return BadRequest(response.Exception);
        }
    }
}
