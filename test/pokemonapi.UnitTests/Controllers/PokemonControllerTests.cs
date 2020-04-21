using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pokemonapi.Controllers;

namespace pokemonapi.UnitTests
{
    [TestClass]
    public class PokemonControllerTests
    {
        [TestMethod]
        public void InitialisePokemonController()
        {
            // Given

            ILogger<PokemonController> _logger = new NullLogger<PokemonController>();

            // When

            _ = new PokemonController(_logger);

        }
    }
}
