using System;
using Microsoft.Extensions.Logging;
using pokemonapi.Models;
using pokemonapi.Models.PokeApi;
using pokemonapi.Services.Interfaces;
using pokemonapi.Services.Refit;
using Refit;
using System.Linq;
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
                return apiException.Content == "Not Found"
                    ? new PokemonResponse("Pokemon not found.")
                    : new PokemonResponse($"Failed to retrieve pokemon. Try again later. Error description: {apiException.Content}");
            }
            catch (Exception exception)
            {
                return new PokemonResponse($"Failed to retrieve pokemon. Try again later. Error description: {exception.Message}");
            }

            //Get Pokemon description

            string pokemonDescriptionEnglish = null;
            try
            {
                PokeApiSpeciesResponse pokeApiSpeciesResponse = await _pokeApiService.GetPokemonSpecies(pokeApiPokemonResponse.Id);

                pokemonDescriptionEnglish = pokeApiSpeciesResponse.TextEntries?.FirstOrDefault(t => t.Language?.Name == "en")?.Text;

                if (string.IsNullOrEmpty(pokemonDescriptionEnglish))
                {
                    return new PokemonResponse("Pokemon description in English not found.");
                }
            }
            catch (ApiException apiException)
            {
                return apiException.Content == "Not Found" ?
                    new PokemonResponse("Pokemon not found.") :
                    new PokemonResponse($"Failed to retrieve pokemon description. Try again later. Error description: {apiException.Content}");
            }
            catch (Exception exception)
            {
                return new PokemonResponse($"Failed to retrieve pokemon description. Try again later. Error description: {exception.Message}");
            }

            //Translate to Shakespearean

            try
            {
                var shakespeareApiResponse = await _shakespeareService.GetTranslation(HttpUtility.UrlEncode(pokemonDescriptionEnglish));

                string shakespeareTranslation = shakespeareApiResponse?.Contents?.Translated;

                if (string.IsNullOrEmpty(shakespeareTranslation))
                {
                    return new PokemonResponse("Translation not available.");
                }

                return new PokemonResponse(new PokemonSuccessfulResponse
                {
                    Description = HttpUtility.UrlDecode(pokemonDescriptionEnglish),
                    Name = pokemonName
                });
            }
            catch (ApiException apiException)
            {
                return new PokemonResponse($"Failed to translate. Try again later. Error description: {apiException.Content}");
            }
            catch (Exception exception)
            {
                return new PokemonResponse($"Failed to translate. Try again later. Error description: {exception.Message}");
            }
        }
    }
}
