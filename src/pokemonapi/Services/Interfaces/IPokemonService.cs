using pokemonapi.Models;
using System.Threading.Tasks;

namespace pokemonapi.Services.Interfaces
{
    public interface IPokemonService
    {
        Task<PokemonResponse> RetrieveShakespeareanDescription(string pokemonName);
    }
}