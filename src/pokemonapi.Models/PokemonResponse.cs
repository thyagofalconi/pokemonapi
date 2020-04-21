namespace pokemonapi.Models
{
    public class PokemonResponse
    {
        public bool Success { get; set; }
        public PokemonSuccessfulResponse Response { get; set; } 
        public string Exception { get; set; }

        public PokemonResponse(PokemonSuccessfulResponse response)
        {
            Success = true;
            Response = response;
        }
        public PokemonResponse(string exception)
        {
            Success = false;
            Exception = exception;
        }
    }
}
