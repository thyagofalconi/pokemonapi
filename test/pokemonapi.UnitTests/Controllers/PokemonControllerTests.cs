using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using pokemonapi.Controllers;
using pokemonapi.Models;
using pokemonapi.Services;

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

        [TestMethod]
        public void GivenASuccessfulResponseFromService_WhenIRequestAShakespeareanDescription_ThenIGetACorrectResponse()
        {
            // Given

            ILogger<PokemonController> logger = Substitute.For<ILogger<PokemonController>>();
            IPokemonService pokemonService = Substitute.For<IPokemonService>();
            PokemonController pokemonController = new PokemonController(logger, pokemonService);
            var pokemon = "charizard";
            var expectedResponse = "expected response";

            pokemonService
                .RetrieveShakespeareanDescription(Arg.Any<string>())
                .Returns(new PokemonSuccessfulResponse(pokemon, expectedResponse));

            // When
            
            var response = pokemonController.RetrieveShakespeareanDescription(pokemon);

            // Then

            Assert.IsTrue(response != null);
            Assert.IsTrue(response.Result is OkObjectResult);
            Assert.IsTrue((response.Result as OkObjectResult).Value is PokemonSuccessfulResponse);
            Assert.IsTrue(((response.Result as OkObjectResult).Value as PokemonSuccessfulResponse).Name == pokemon);
            Assert.IsTrue(((response.Result as OkObjectResult).Value as PokemonSuccessfulResponse).Description == expectedResponse);
        }

        [TestMethod]
        public void GivenAUnsuccessfulResponseFromService_WhenIRequestAShakespeareanDescription_ThenIGetAFailedResponse()
        {
            // Given

            ILogger<PokemonController> logger = Substitute.For<ILogger<PokemonController>>();
            IPokemonService pokemonService = Substitute.For<IPokemonService>();
            PokemonController pokemonController = new PokemonController(logger, pokemonService);
            var pokemon = "charizard";
            var expectedErrorMessage = "expected error message";

            pokemonService
                .RetrieveShakespeareanDescription(Arg.Any<string>())
                .Returns(new PokemonFailedResponse(expectedErrorMessage));

            // When

            var response = pokemonController.RetrieveShakespeareanDescription(pokemon);

            // Then

            Assert.IsTrue(response != null);
            Assert.IsTrue(response.Result is BadRequestObjectResult);
            Assert.IsTrue((response.Result as BadRequestObjectResult).Value is string);
            Assert.IsTrue((response.Result as BadRequestObjectResult).Value == expectedErrorMessage);
        }

        [TestMethod]
        public void GivenAnEmptyRequestParameter_WhenIRequestAShakespeareanDescription_ThenIGetAFailedResponse()
        {
            // Given

            ILogger<PokemonController> logger = Substitute.For<ILogger<PokemonController>>();
            IPokemonService pokemonService = Substitute.For<IPokemonService>();
            PokemonController pokemonController = new PokemonController(logger, pokemonService);
            var pokemon = "";

            // When

            var response = pokemonController.RetrieveShakespeareanDescription(pokemon);

            // Then

            Assert.IsTrue(response != null);
            Assert.IsTrue(response.Result is BadRequestObjectResult);
        }
    }
}
