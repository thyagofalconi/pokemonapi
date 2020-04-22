using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using pokemonapi.Models;
using pokemonapi.Models.PokeApi;
using pokemonapi.Models.ShakespeareApi;
using pokemonapi.Services;
using pokemonapi.Services.Refit;
using Refit;

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

        [TestMethod]
        public void GivenANotExistingPokemonNameIsProvided_WhenITryToRetrieveTheShakespeareanDescription_ThenIGetAnErrorMessage()
        {
            // Given

            ILogger<PokemonService> logger = Substitute.For<ILogger<PokemonService>>();
            IPokeApiService pokeApiService = Substitute.For<IPokeApiService>();
            IShakespeareService shakespeareService = Substitute.For<IShakespeareService>();
            PokemonService pokemonService = new PokemonService(logger, pokeApiService, shakespeareService);
            var pokemon = "pokemon name";

            pokeApiService
                .GetPokemon(Arg.Any<string>())
                .Throws(Task.FromException(ApiException.Create(null, null, new HttpResponseMessage(HttpStatusCode.NotFound)).Result).Exception);

            // When

            var response = pokemonService.RetrieveShakespeareanDescription(pokemon).GetAwaiter().GetResult();

            // Then

            Assert.IsTrue(response != null);
            Assert.IsTrue(response is PokemonFailedResponse);
            Assert.IsTrue((response as PokemonFailedResponse).Success == false);
            Assert.IsTrue((response as PokemonFailedResponse).Exception != null);
            Assert.IsTrue((response as PokemonFailedResponse).Exception == "Failed to retrieve pokemon. Try again later. Error description: One or more errors occurred. (Response status code does not indicate success: 404 (Not Found).)");
        }

        [TestMethod]
        public void GivenAnUnknownErrorIsThrownByThePokeApi_WhenITryToRetrieveTheShakespeareanDescription_ThenIGetAnErrorMessage()
        {
            // Given

            ILogger<PokemonService> logger = Substitute.For<ILogger<PokemonService>>();
            IPokeApiService pokeApiService = Substitute.For<IPokeApiService>();
            IShakespeareService shakespeareService = Substitute.For<IShakespeareService>();
            PokemonService pokemonService = new PokemonService(logger, pokeApiService, shakespeareService);
            var pokemon = "pokemon name";

            pokeApiService
                .GetPokemon(Arg.Any<string>())
                .Throws(new Exception());

            // When

            var response = pokemonService.RetrieveShakespeareanDescription(pokemon).GetAwaiter().GetResult();

            // Then

            Assert.IsTrue(response != null);
            Assert.IsTrue(response is PokemonFailedResponse);
            Assert.IsTrue(response.Success == false);
            Assert.IsTrue((response as PokemonFailedResponse).Exception != null);
            Assert.IsTrue((response as PokemonFailedResponse).Exception == "Failed to retrieve pokemon. Try again later. Error description: Exception of type 'System.Exception' was thrown.");
        }

        [TestMethod]
        public void GivenAnUnknownErrorOfTypeApiExceptionIsThrownByThePokeApi_WhenITryToRetrieveTheShakespeareanDescription_ThenIGetAnErrorMessage()
        {
            // Given

            ILogger<PokemonService> logger = Substitute.For<ILogger<PokemonService>>();
            IPokeApiService pokeApiService = Substitute.For<IPokeApiService>();
            IShakespeareService shakespeareService = Substitute.For<IShakespeareService>();
            PokemonService pokemonService = new PokemonService(logger, pokeApiService, shakespeareService);
            var pokemon = "pokemon name";


            pokeApiService
                .GetPokemon(Arg.Any<string>())
                .Throws(Task.FromException(ApiException.Create(null, null, new HttpResponseMessage(HttpStatusCode.InternalServerError)).Result).Exception);

            // When

            var response = pokemonService.RetrieveShakespeareanDescription(pokemon).GetAwaiter().GetResult();

            // Then

            Assert.IsTrue(response != null);
            Assert.IsTrue(response is PokemonFailedResponse);
            Assert.IsTrue(response.Success == false);
            Assert.IsTrue((response as PokemonFailedResponse).Exception != null);
            Assert.IsTrue((response as PokemonFailedResponse).Exception == "Failed to retrieve pokemon. Try again later. Error description: One or more errors occurred. (Response status code does not indicate success: 500 (Internal Server Error).)");
        }

        [TestMethod]
        public void GivenAPokemonThatDoesntHaveDescriptionIsProvided_WhenITryToRetrieveTheShakespeareanDescription_ThenIGetAnErrorMessage()
        {
            // Given

            ILogger<PokemonService> logger = Substitute.For<ILogger<PokemonService>>();
            IPokeApiService pokeApiService = Substitute.For<IPokeApiService>();
            IShakespeareService shakespeareService = Substitute.For<IShakespeareService>();
            PokemonService pokemonService = new PokemonService(logger, pokeApiService, shakespeareService);
            var pokemon = "pokemon name";

            pokeApiService
                .GetPokemon(pokemon)
                .Returns(Task.FromResult(new PokeApiPokemonResponse { Id = pokemon }));

            pokeApiService
                .GetPokemonSpecies(pokemon)
                .Throws(Task.FromException(ApiException.Create(null, null, new HttpResponseMessage(HttpStatusCode.NotFound)).Result).Exception);

            // When

            var response = pokemonService.RetrieveShakespeareanDescription(pokemon).GetAwaiter().GetResult();

            // Then

            Assert.IsTrue(response != null);
            Assert.IsTrue(response is PokemonFailedResponse);
            Assert.IsTrue(response.Success == false);
            Assert.IsTrue((response as PokemonFailedResponse).Exception != null);
            Assert.IsTrue((response as PokemonFailedResponse).Exception == "Failed to retrieve pokemon description. Try again later. Error description: One or more errors occurred. (Response status code does not indicate success: 404 (Not Found).)");
        }

        [TestMethod]
        public void GivenAnUnknownErrorIsThrownByThePokeApiWhenTryingToRetrieveAPokemonDescription_WhenITryToRetrieveTheShakespeareanDescription_ThenIGetAnErrorMessage()
        {
            // Given

            ILogger<PokemonService> logger = Substitute.For<ILogger<PokemonService>>();
            IPokeApiService pokeApiService = Substitute.For<IPokeApiService>();
            IShakespeareService shakespeareService = Substitute.For<IShakespeareService>();
            PokemonService pokemonService = new PokemonService(logger, pokeApiService, shakespeareService);
            var pokemon = "pokemon name";

            pokeApiService
                .GetPokemon(pokemon)
                .Returns(Task.FromResult(new PokeApiPokemonResponse { Id = pokemon }));

            pokeApiService
                .GetPokemonSpecies(pokemon)
                .Throws(new Exception());

            // When

            var response = pokemonService.RetrieveShakespeareanDescription(pokemon).GetAwaiter().GetResult();

            // Then

            Assert.IsTrue(response != null);
            Assert.IsTrue(response is PokemonFailedResponse);
            Assert.IsTrue(response.Success == false);
            Assert.IsTrue((response as PokemonFailedResponse).Exception != null);
            Assert.IsTrue((response as PokemonFailedResponse).Exception == "Failed to retrieve pokemon description. Try again later. Error description: Exception of type 'System.Exception' was thrown.");
        }

        [TestMethod]
        public void GivenAnUnknownErrorOfTypeApiExceptionIsThrownByThePokeApiWhenTryingToRetrieveAPokemonDescription_WhenITryToRetrieveTheShakespeareanDescription_ThenIGetAnErrorMessage()
        {
            // Given

            ILogger<PokemonService> logger = Substitute.For<ILogger<PokemonService>>();
            IPokeApiService pokeApiService = Substitute.For<IPokeApiService>();
            IShakespeareService shakespeareService = Substitute.For<IShakespeareService>();
            PokemonService pokemonService = new PokemonService(logger, pokeApiService, shakespeareService);
            var pokemon = "pokemon name";

            pokeApiService
                .GetPokemon(pokemon)
                .Returns(Task.FromResult(new PokeApiPokemonResponse { Id = pokemon }));

            pokeApiService
                .GetPokemonSpecies(pokemon)
                .Throws(Task.FromException(ApiException.Create(null, null, new HttpResponseMessage(HttpStatusCode.InternalServerError)).Result).Exception);

            // When

            var response = pokemonService.RetrieveShakespeareanDescription(pokemon).GetAwaiter().GetResult();

            // Then

            Assert.IsTrue(response != null);
            Assert.IsTrue(response is PokemonFailedResponse);
            Assert.IsTrue(response.Success == false);
            Assert.IsTrue((response as PokemonFailedResponse).Exception != null);
            Assert.IsTrue((response as PokemonFailedResponse).Exception == "Failed to retrieve pokemon description. Try again later. Error description: One or more errors occurred. (Response status code does not indicate success: 500 (Internal Server Error).)");
        }

        [TestMethod]
        public void GivenAPokemonThatDoesntHaveADescriptionInEnglish_WhenITryToRetrieveTheShakespeareanDescription_ThenIGetAnErrorMessage()
        {
            // Given

            ILogger<PokemonService> logger = Substitute.For<ILogger<PokemonService>>();
            IPokeApiService pokeApiService = Substitute.For<IPokeApiService>();
            IShakespeareService shakespeareService = Substitute.For<IShakespeareService>();
            PokemonService pokemonService = new PokemonService(logger, pokeApiService, shakespeareService);
            var pokemon = "pokemon name";
            var pokeApiSpeciesResponse = new PokeApiSpeciesResponse
            {
                TextEntries = new List<PokeApiSpeciesTextEntry>
                {
                    new PokeApiSpeciesTextEntry
                    {
                        Text = string.Empty,
                        Language = new PokeApiSpeciesTextLanguage
                        {
                            Name = "fr"
                        }
                    }
                }
            };

            pokeApiService
                .GetPokemon(pokemon)
                .Returns(Task.FromResult(new PokeApiPokemonResponse { Id = pokemon }));

            pokeApiService
                .GetPokemonSpecies(pokemon)
                .Returns(Task.FromResult(pokeApiSpeciesResponse));

            // When

            var response = pokemonService.RetrieveShakespeareanDescription(pokemon).GetAwaiter().GetResult();

            // Then

            Assert.IsTrue(response != null);
            Assert.IsTrue(response is PokemonFailedResponse);
            Assert.IsTrue(response.Success == false);
            Assert.IsTrue((response as PokemonFailedResponse).Exception != null);
            Assert.IsTrue((response as PokemonFailedResponse).Exception == "Pokemon description in English not found.");
        }
        
        [TestMethod]
        public void  GivenAnUnknownErrorOfTypeApiExceptionIsThrownByTheShakespeareApi_WhenITryToRetrieveTheShakespeareanDescription_ThenIGetAnErrorMessage()
        {
            // Given

            ILogger<PokemonService> logger = Substitute.For<ILogger<PokemonService>>();
            IPokeApiService pokeApiService = Substitute.For<IPokeApiService>();
            IShakespeareService shakespeareService = Substitute.For<IShakespeareService>();
            PokemonService pokemonService = new PokemonService(logger, pokeApiService, shakespeareService);
            var pokemon = "pokemon name";
            var pokeApiSpeciesResponse = new PokeApiSpeciesResponse
            {
                TextEntries = new List<PokeApiSpeciesTextEntry>
                {
                    new PokeApiSpeciesTextEntry
                    {
                        Text = "text",
                        Language = new PokeApiSpeciesTextLanguage
                        {
                            Name = "en"
                        }
                    }
                }
            };

            pokeApiService
                .GetPokemon(pokemon)
                .Returns(Task.FromResult(new PokeApiPokemonResponse { Id = pokemon }));

            pokeApiService
                .GetPokemonSpecies(pokemon)
                .Returns(Task.FromResult(pokeApiSpeciesResponse));

            shakespeareService
                .GetTranslation(Arg.Any<string>())
                .Throws(Task.FromException(ApiException
                    .Create(null, null, new HttpResponseMessage(HttpStatusCode.InternalServerError)).Result).Exception);

            // When

            var response = pokemonService.RetrieveShakespeareanDescription(pokemon).GetAwaiter().GetResult();

            // Then

            Assert.IsTrue(response != null);
            Assert.IsTrue(response is PokemonFailedResponse);
            Assert.IsTrue(response.Success == false);
            Assert.IsTrue((response as PokemonFailedResponse).Exception != null);
            Assert.IsTrue((response as PokemonFailedResponse).Exception == "Failed to translate. Try again later. Error description: One or more errors occurred. (Response status code does not indicate success: 500 (Internal Server Error).)");
        }

        [TestMethod]
        public void GivenAnUnknownErrorIsThrownByTheShakespeareApi_WhenITryToRetrieveTheShakespeareanDescription_ThenIGetAnErrorMessage()
        {
            // Given

            ILogger<PokemonService> logger = Substitute.For<ILogger<PokemonService>>();
            IPokeApiService pokeApiService = Substitute.For<IPokeApiService>();
            IShakespeareService shakespeareService = Substitute.For<IShakespeareService>();
            PokemonService pokemonService = new PokemonService(logger, pokeApiService, shakespeareService);
            var pokemon = "pokemon name";
            var pokeApiSpeciesResponse = new PokeApiSpeciesResponse
            {
                TextEntries = new List<PokeApiSpeciesTextEntry>
                {
                    new PokeApiSpeciesTextEntry
                    {
                        Text = "text",
                        Language = new PokeApiSpeciesTextLanguage
                        {
                            Name = "en"
                        }
                    }
                }
            };

            pokeApiService
                .GetPokemon(pokemon)
                .Returns(Task.FromResult(new PokeApiPokemonResponse { Id = pokemon }));

            pokeApiService
                .GetPokemonSpecies(pokemon)
                .Returns(Task.FromResult(pokeApiSpeciesResponse));

            shakespeareService
                .GetTranslation(Arg.Any<string>())
                .Throws(new Exception());

            // When

            var response = pokemonService.RetrieveShakespeareanDescription(pokemon).GetAwaiter().GetResult();

            // Then

            Assert.IsTrue(response != null);
            Assert.IsTrue(response is PokemonFailedResponse);
            Assert.IsTrue(response.Success == false);
            Assert.IsTrue((response as PokemonFailedResponse).Exception != null);
            Assert.IsTrue((response as PokemonFailedResponse).Exception == "Failed to translate. Try again later. Error description: Exception of type 'System.Exception' was thrown.");
        }
        
        [TestMethod]
        public void GivenShakespeareApiDoesntReturnAValidTranslation_WhenITryToRetrieveTheShakespeareanDescription_ThenIGetAnErrorMessage()
        {
            // Given

            ILogger<PokemonService> logger = Substitute.For<ILogger<PokemonService>>();
            IPokeApiService pokeApiService = Substitute.For<IPokeApiService>();
            IShakespeareService shakespeareService = Substitute.For<IShakespeareService>();
            PokemonService pokemonService = new PokemonService(logger, pokeApiService, shakespeareService);
            var pokemon = "pokemon name";
            var pokeApiSpeciesResponse = new PokeApiSpeciesResponse
            {
                TextEntries = new List<PokeApiSpeciesTextEntry>
                {
                    new PokeApiSpeciesTextEntry
                    {
                        Text = "text",
                        Language = new PokeApiSpeciesTextLanguage
                        {
                            Name = "en"
                        }
                    }
                }
            };
            var shakespeareApiResponse = new ShakespeareApiResponse
            {
                Contents = new ShakespeareApiContentResponse
                {
                    Translated = null
                }
            };

            pokeApiService
                .GetPokemon(pokemon)
                .Returns(Task.FromResult(new PokeApiPokemonResponse { Id = pokemon }));

            pokeApiService
                .GetPokemonSpecies(pokemon)
                .Returns(Task.FromResult(pokeApiSpeciesResponse));

            shakespeareService
                .GetTranslation(Arg.Any<string>())
                .Returns(Task.FromResult(shakespeareApiResponse));

            // When

            var response = pokemonService.RetrieveShakespeareanDescription(pokemon).GetAwaiter().GetResult();

            // Then

            Assert.IsTrue(response != null);
            Assert.IsTrue(response is PokemonFailedResponse);
            Assert.IsTrue(response.Success == false);
            Assert.IsTrue((response as PokemonFailedResponse).Exception != null);
            Assert.IsTrue((response as PokemonFailedResponse).Exception == "Translation not available.");
        }

        [TestMethod]
        public void GivenAValidPokemonAndTranslation_WhenITryToRetrieveTheShakespeareanDescription_ThenIGetAValidResponse()
        {
            // Given

            ILogger<PokemonService> logger = Substitute.For<ILogger<PokemonService>>();
            IPokeApiService pokeApiService = Substitute.For<IPokeApiService>();
            IShakespeareService shakespeareService = Substitute.For<IShakespeareService>();
            PokemonService pokemonService = new PokemonService(logger, pokeApiService, shakespeareService);
            var pokemon = "pokemon name";
            var expectedTranslation = "test";
            var pokeApiSpeciesResponse = new PokeApiSpeciesResponse
            {
                TextEntries = new List<PokeApiSpeciesTextEntry>
                {
                    new PokeApiSpeciesTextEntry
                    {
                        Text = expectedTranslation,
                        Language = new PokeApiSpeciesTextLanguage
                        {
                            Name = "en"
                        }
                    }
                }
            };
            var shakespeareApiResponse = new ShakespeareApiResponse
            {
                Contents = new ShakespeareApiContentResponse
                {
                    Translated = expectedTranslation
                }
            };

            pokeApiService
                .GetPokemon(pokemon)
                .Returns(Task.FromResult(new PokeApiPokemonResponse { Id = pokemon }));

            pokeApiService
                .GetPokemonSpecies(pokemon)
                .Returns(Task.FromResult(pokeApiSpeciesResponse));

            shakespeareService
                .GetTranslation(Arg.Any<string>())
                .Returns(Task.FromResult(shakespeareApiResponse));

            // When

            var response = pokemonService.RetrieveShakespeareanDescription(pokemon).GetAwaiter().GetResult();

            // Then

            Assert.IsTrue(response != null);
            Assert.IsTrue(response is PokemonSuccessfulResponse);
            Assert.IsTrue(response.Success == true);
            Assert.IsTrue((response as PokemonSuccessfulResponse).Description == expectedTranslation);
            Assert.IsTrue((response as PokemonSuccessfulResponse).Name == pokemon);
        }

    }
}
