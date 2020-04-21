namespace pokemonapi.Models
{
    public class PokemonResponse
    {
        public bool Success { get; set; }
        public object Response { get; set; }

        public PokemonResponse(bool success, object response)
        {
            Success = success;
            Response = response;
        }
    }
}
