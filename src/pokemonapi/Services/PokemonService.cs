using System;
using Microsoft.Extensions.Logging;
using pokemonapi.Models;
using pokemonapi.Models.PokeApi;
using pokemonapi.Services.Refit;
using Refit;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace pokemonapi.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly ILogger<PokemonService> _logger;
        private readonly IPokeApiService _pokeApiService;
        private readonly IShakespeareService _shakespeareService;

        public PokemonService(ILogger<PokemonService> logger, IPokeApiService pokeApiService, IShakespeareService shakespeareService)
        {
            _logger = logger;
            _pokeApiService = pokeApiService;
            _shakespeareService = shakespeareService;
        }

        public async Task<PokemonResponse> RetrieveShakespeareanDescription(string pokemonName)
        {
            //Get Pokemon

            PokeApiPokemonResponse pokeApiPokemonResponse = null;
            try
            {
                pokeApiPokemonResponse = await _pokeApiService.GetPokemon(pokemonName);
            }
            catch (ApiException apiException)
            {
                return apiException.Content == HttpStatusCode.NotFound.ToString()
                    ? new PokemonFailedResponse("Pokemon not found.", HttpStatusCode.NotFound)
                    : new PokemonFailedResponse($"Failed to retrieve pokemon. Try again later. Error description: {apiException.Content}");
            }
            catch (Exception exception)
            {
                return new PokemonFailedResponse($"Failed to retrieve pokemon. Try again later. Error description: {exception.Message}");
            }

            //Get Pokemon description

            string pokemonDescriptionEnglish = null;
            try
            {
                PokeApiSpeciesResponse pokeApiSpeciesResponse = await _pokeApiService.GetPokemonSpecies(pokeApiPokemonResponse.Id);

                pokemonDescriptionEnglish = pokeApiSpeciesResponse.TextEntries?.FirstOrDefault(t => t.Language?.Name == "en")?.Text;

                if (string.IsNullOrEmpty(pokemonDescriptionEnglish))
                {
                    return new PokemonFailedResponse("Pokemon description in English not found.");
                }
            }
            catch (ApiException apiException)
            {
                return apiException.Content == HttpStatusCode.NotFound.ToString() ?
                    new PokemonFailedResponse("Pokemon not found.", HttpStatusCode.NotFound) :
                    new PokemonFailedResponse($"Failed to retrieve pokemon description. Try again later. Error description: {apiException.Content}");
            }
            catch (Exception exception)
            {
                return new PokemonFailedResponse($"Failed to retrieve pokemon description. Try again later. Error description: {exception.Message}");
            }

            //Translate to Shakespearean

            try
            {
                var shakespeareApiResponse = await _shakespeareService.GetTranslation(HttpUtility.UrlEncode(pokemonDescriptionEnglish));

                string shakespeareTranslation = shakespeareApiResponse?.Contents?.Translated;

                if (string.IsNullOrEmpty(shakespeareTranslation))
                {
                    return new PokemonFailedResponse("Translation not available.", HttpStatusCode.NotFound);
                }

                return new PokemonSuccessfulResponse(pokemonName, HttpUtility.UrlDecode(pokemonDescriptionEnglish));
            }
            catch (ApiException apiException)
            {
                return new PokemonFailedResponse($"Failed to translate. Try again later. Error description: {apiException.Content}");
            }
            catch (Exception exception)
            {
                return new PokemonFailedResponse($"Failed to translate. Try again later. Error description: {exception.Message}");
            }
        }
    }
}
