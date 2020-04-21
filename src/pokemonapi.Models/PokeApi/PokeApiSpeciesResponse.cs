using System.Collections.Generic;
using Newtonsoft.Json;

namespace pokemonapi.Models.PokeApi
{
    public class PokeApiSpeciesResponse
    {
        [JsonProperty("flavor_text_entries")]
        public List<PokeApiSpeciesTextEntry> TextEntries { get; set; }
    }
}