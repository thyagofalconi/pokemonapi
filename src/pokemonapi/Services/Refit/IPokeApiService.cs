﻿using pokemonapi.Models.PokeApi;
using Refit;
using System.Threading.Tasks;

namespace pokemonapi.Services.Refit
{
    public interface IPokeApiService
    {
        [Get("/api/v2/pokemon/{pokemonName}")]
        Task<PokeApiPokemonResponse> GetPokemon(string pokemonName);

        [Get("/api/v2/pokemon-species/{pokemonId}")]
        Task<PokeApiSpeciesResponse> GetPokemonSpecies(string pokemonId);
    }
}