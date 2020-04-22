namespace pokemonapi.Models
{
    public class PokemonSuccessfulResponse : PokemonResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public PokemonSuccessfulResponse(string name, string description)
        {
            Success = true;
            Name = name;
            Description = description;
        }
    }
}