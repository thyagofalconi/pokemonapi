using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using pokemonapi.Controllers;
using pokemonapi.Services;
using pokemonapi.Services.Interfaces;

namespace pokemonapi.UnitTests.Controllers
{
    [TestClass]
    public class PokemonControllerTests
    {
        [TestMethod]
        public void InitialisePokemonController()
        {
            // Given

            ILogger<PokemonController> logger = Substitute.For<ILogger<PokemonController>>();
            IPokemonService pokemonService = Substitute.For<IPokemonService>();

            // When

            _ = new PokemonController(logger, pokemonService);

        }
    }
}
