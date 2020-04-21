using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pokemonapi.Controllers;
using pokemonapi.Services;
using pokemonapi.Services.Interfaces;

namespace pokemonapi.UnitTests
{
    [TestClass]
    public class PokemonControllerTests
    {
        [TestMethod]
        public void InitialisePokemonController()
        {
            // Given

            ILogger<PokemonController> logger = new NullLogger<PokemonController>();
            IPokemonService pokemonService = new PokemonService();

            // When

            _ = new PokemonController(logger, pokemonService);

        }
    }
}
