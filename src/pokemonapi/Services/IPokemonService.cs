using System.Threading.Tasks;
using pokemonapi.Models;

namespace pokemonapi.Services
{
    public interface IPokemonService
    {
        Task<PokemonResponse> RetrieveShakespeareanDescription(string pokemonName);
    }
}