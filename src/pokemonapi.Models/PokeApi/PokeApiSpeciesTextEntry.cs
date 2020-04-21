using Newtonsoft.Json;

namespace pokemonapi.Models.PokeApi
{
    public class PokeApiSpeciesTextEntry
    {
        [JsonProperty("flavor_text")]
        public string Text { get; set; }

        [JsonProperty("language")]
        public PokeApiSpeciesTextLanguage Language { get; set; }
    }
}