using Microsoft.Extensions.Logging;
using pokemonapi.Models;
using pokemonapi.Models.PokeApi;
using pokemonapi.Services.Interfaces;
using pokemonapi.Services.Refit;
using Refit;
using System.Linq;
using System.Threading.Tasks;

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
            PokeApiPokemonResponse pokeApiPokemonResponse = null;
            try
            {
                pokeApiPokemonResponse = await _pokeApiService.GetPokemon(pokemonName);
            }
            catch (ApiException apiException)
            {
                return apiException.Content == "Not Found"
                    ? new PokemonResponse(false, "Pokemon not found.")
                    : new PokemonResponse(false,
                        $"Failed to retrieve pokemon. Try again later. Error description: {apiException.Content}");
            }

            string pokemonDescriptionEnglish = null;
            try
            {
                PokeApiSpeciesResponse pokeApiSpeciesResponse = await _pokeApiService.GetPokemonSpecies(pokeApiPokemonResponse.Id);

                pokemonDescriptionEnglish = pokeApiSpeciesResponse.TextEntries?.FirstOrDefault(t => t.Language?.Name == "en")?.Text;

                if (string.IsNullOrEmpty(pokemonDescriptionEnglish))
                {
                    return new PokemonResponse(false, "Pokemon description not found.");
                }
            }
            catch (ApiException apiException)
            {
                return apiException.Content == "Not Found" ?
                    new PokemonResponse(false, "Pokemon not found.") :
                    new PokemonResponse(false, $"Failed to retrieve pokemon description. Try again later. Error description: {apiException.Content}");
            }

            try
            {
                var shakespeareApiResponse = await _shakespeareService.GetTranslation(System.Web.HttpUtility.UrlEncode(pokemonDescriptionEnglish));

                string shakespeareTranslation = shakespeareApiResponse?.Contents?.Translated;

                if (string.IsNullOrEmpty(shakespeareTranslation))
                {
                    return new PokemonResponse(false, "Translation not available.");
                }

                return new PokemonResponse(true, shakespeareTranslation);
            }
            catch (ApiException apiException)
            {
                return new PokemonResponse(false, $"Failed to translate. Try again later. Error description: {apiException.Content}");
            }
        }
    }
}
