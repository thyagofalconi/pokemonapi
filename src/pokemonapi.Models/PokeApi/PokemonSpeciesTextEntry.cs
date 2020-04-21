using Newtonsoft.Json;

namespace pokemonapi.Models.PokeApi
{
    public class PokemonSpeciesTextEntry
    {
        [JsonProperty("flavor_text")]
        public string Text { get; set; }

        [JsonProperty("language")]
        public PokemonSpeciesTextLanguage Language { get; set; }
    }
}