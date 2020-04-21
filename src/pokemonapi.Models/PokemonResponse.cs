using System;

namespace pokemonapi.Models
{
    public class PokemonResponse
    {
        public bool Success { get; set; }
        public object Response { get; set; }
        public object Exception { get; set; }
    }
}
