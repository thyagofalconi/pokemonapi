using System.Net;

namespace pokemonapi.Models
{
    public class PokemonFailedResponse : PokemonResponse
    {
        public string Exception { get; set; }
        public HttpStatusCode? HttpStatusCode { get; set; }

        public PokemonFailedResponse(string exception)
        {
            Success = false;
            Exception = exception;
        }

        public PokemonFailedResponse(string exception, HttpStatusCode httpStatusCode)
        {
            Success = false;
            Exception = exception;
            HttpStatusCode = httpStatusCode;
        }
    }
}