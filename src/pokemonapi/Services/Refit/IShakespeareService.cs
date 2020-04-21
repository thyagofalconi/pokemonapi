using pokemonapi.Models.ShakespeareApi;
using Refit;
using System.Threading.Tasks;

namespace pokemonapi.Services.Refit
{
    public interface IShakespeareService
    {
        [Get("/shakespeare.json?text={text}")]
        Task<ShakespeareApiResponse> GetTranslation(string text);
    }
}