using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace pokemonapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly ILogger<PokemonController> _logger;

        public PokemonController(ILogger<PokemonController> logger)
        {
            _logger = logger;
        }

        //GET endpoint: /pokemon/<pokemon name>
    }
}
