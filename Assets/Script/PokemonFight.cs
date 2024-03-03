using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PokemonFight : MonoBehaviour
{
    List<Pokemon> pokemonList;
    Pokemon firstPokemon;
    Pokemon secondPokemon;
    PokemonList pokemonListClass;
    [SerializeField] UIManager uiManager;

    public void Awake()
    {
        pokemonListClass = new PokemonList();
    }
    public void StartFight()
    {
        StartCoroutine(PreFight());
    }

    private IEnumerator PreFight()
    {
        ChooseTwoPokemons();
        firstPokemon.PostInit(uiManager.GetLevel());
        secondPokemon.PostInit(uiManager.GetLevel());
        GameObject.Find("Pokemons").GetComponent<UIManager>().PreFightVisual(new Pokemon[] { firstPokemon, secondPokemon });
        yield return new WaitForSeconds(5);
        StartCoroutine(Fight());
    }

    private IEnumerator Fight()
    {
        uiManager.FightVisual();
        int turn = 0;
        while (firstPokemon.GetHealth() > 0 && secondPokemon.GetHealth() > 0)
        {
            turn++;
            if (turn % 2 == 1) //FIRST POKEMON
            {
                Debug.Log("------ TURN " + turn / 2 + "------");
                uiManager.IncrementTurn(turn / 2 + 1);
                uiManager.DamageAnimation(secondPokemon);
                firstPokemon.Attack(secondPokemon);
                uiManager.pokemonUI2.OnHealthChange(secondPokemon.GetHealth());
                yield return new WaitForSeconds(2);
                
            }
            else if (turn % 2 == 0) //SECOND POKEMON
            {
                uiManager.IncrementTurn(turn / 2 + 1);
                secondPokemon.Attack(firstPokemon);
                uiManager.DamageAnimation(firstPokemon);
                uiManager.pokemonUI1.OnHealthChange(firstPokemon.GetHealth());
                yield return new WaitForSeconds(2);
            }
        }
        if(turn % 2 == 1) PostFight(firstPokemon);
        else if (turn % 2 == 0) PostFight(secondPokemon);
    }

    private void PostFight(Pokemon winner)
    {
        Debug.Log($"{winner.GetName()} gagne le combat!");
        uiManager.PostFightVisual();
    }

    private void ChooseTwoPokemons()
    {
        Pokemon pokemon1;
        Pokemon pokemon2;
        pokemonList = pokemonListClass.LoadPokemons();
        List<Pokemon> pokemonListModifiable = new(pokemonList);
        int rng = Random.Range(0, pokemonListModifiable.Count);
        pokemon1 = pokemonListModifiable[rng];
        pokemonListModifiable.Remove(pokemon1);
        if(pokemonListModifiable.Count != 0)
        {
            rng = Random.Range(0, pokemonListModifiable.Count);
            pokemon2 = pokemonListModifiable[rng];
            if (pokemon1.GetSpeed() > pokemon2.GetSpeed())
            {
                firstPokemon = pokemon1;
                secondPokemon = pokemon2;
            }
            else
            {
                firstPokemon = pokemon2;
                secondPokemon = pokemon1;
            }
        }
        else
        {
            firstPokemon = pokemon1;
            secondPokemon = pokemon1;
        }
    } 
}
