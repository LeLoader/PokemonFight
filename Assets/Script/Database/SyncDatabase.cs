using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncDatabase : MonoBehaviour
{
    private async void Start()
    {
        //MoveDatabaseManager.Instance.movedb.moves.Clear(); //uncomment to clear
        //PokemonDatabaseManager.Instance.pokemondb.pokemons.Clear(); //uncomment to clear

        MoveDatabaseManager.Instance.SortedMove = MoveDatabaseManager.Instance.movedb.moves;
        PokemonDatabaseManager.Instance.SortedPokemon = PokemonDatabaseManager.Instance.pokemondb.pokemons;

        //await MoveDatabaseManager.Instance.LoadMoves(919); //Uncomment to reload (will only find 571)
        var pqmonlist = await PokemonDatabaseManager.Instance.LoadPokemons(10000);
        PokemonFiche.Instance.pokemonList = pqmonlist.ConvertAll(pokemon => new Pokemon(pokemon.Id, pokemon.Name, pokemon.BaseStats.Health, pokemon.BaseStats.Attack, pokemon.BaseStats.Defense, pokemon.BaseStats.SpAttack, pokemon.BaseStats.SpDefense, pokemon.BaseStats.Speed, pokemon.type1, pokemon.type2));

        PokemonFiche.Instance.Setup();
    }
}
