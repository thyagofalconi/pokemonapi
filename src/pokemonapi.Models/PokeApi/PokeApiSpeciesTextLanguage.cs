using Newtonsoft.Json;

namespace pokemonapi.Models.PokeApi
{
    public class PokeApiSpeciesTextLanguage
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}