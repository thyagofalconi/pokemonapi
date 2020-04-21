using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using pokemonapi.Services;
using pokemonapi.Services.Refit;

namespace pokemonapi.UnitTests.Services
{
    [TestClass]
    public class PokemonServiceTests
    {
        [TestMethod]
        public void InitialisePokemonService()
        {
            // Given

            ILogger<PokemonService> logger = Substitute.For<ILogger<PokemonService>>();
            IPokeApiService pokeApiService = Substitute.For<IPokeApiService>();
            IShakespeareService shakespeareService = Substitute.For<IShakespeareService>();

            // When

            _ = new PokemonService(logger, pokeApiService, shakespeareService);
        }
    }
}
