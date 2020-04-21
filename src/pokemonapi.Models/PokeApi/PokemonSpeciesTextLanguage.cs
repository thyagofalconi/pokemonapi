using Newtonsoft.Json;

namespace pokemonapi.Models.PokeApi
{
    public class PokemonSpeciesTextLanguage
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}