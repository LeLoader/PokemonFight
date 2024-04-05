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
    [SerializeField] UIManager uiManager;
    [SerializeField] GameObject fichePokemon; 

    public void StartFight()
    {
        StartCoroutine(PreFight());
    }

    private IEnumerator PreFight()
    {
        fichePokemon.SetActive(false);
        ChooseTwoPokemons();
        uiManager.PreFightVisual(new Pokemon[] { firstPokemon, secondPokemon });
        yield return new WaitForSeconds(5);
        StartCoroutine(Fight());
    }

    private IEnumerator Fight()
    {
        uiManager.FightVisual();
        int turn = 0;
        while (firstPokemon.CurrentHealth > 0 && secondPokemon.CurrentHealth > 0)
        {
            turn++;
            if (turn % 2 == 1) //FIRST POKEMON
            {
                Debug.Log("------ TURN " + (turn + 1) / 2 + "------");
                uiManager.IncrementTurn((turn + 1) / 2);
                uiManager.DamageAnimation(secondPokemon);
                firstPokemon.AttackPokemon(secondPokemon);
                uiManager.pokemonUI2.OnHealthChange(secondPokemon.CurrentHealth);
                yield return new WaitForSeconds(2);
                
            }
            else if (turn % 2 == 0) //SECOND POKEMON
            {
                uiManager.IncrementTurn((turn + 1 )/ 2);
                secondPokemon.AttackPokemon(firstPokemon);
                uiManager.DamageAnimation(firstPokemon);
                uiManager.pokemonUI1.OnHealthChange(firstPokemon.CurrentHealth);
                yield return new WaitForSeconds(2);
            }
        }
        if(turn % 2 == 1) PostFight(firstPokemon);
        else if (turn % 2 == 0) PostFight(secondPokemon);
    }

    private void PostFight(Pokemon winner)
    {
        Debug.Log($"{winner.Name} gagne le combat!");
        uiManager.PostFightVisual();
        fichePokemon.SetActive(true);
    }

    private async void ChooseTwoPokemons()
    {
        pokemonList = await PokemonDatabase.Instance.LoadPokemons(10000);
        List<Pokemon> pokemonListModifiable = new(pokemonList);
        Pokemon pokemon1 = fichePokemon.GetComponent<PokemonFiche>().ActualPokemon;
        Pokemon pokemon2;
        pokemonListModifiable.Remove(pokemon1);
        if(pokemonListModifiable.Count != 0)
        {
            int rng = Random.Range(0, pokemonListModifiable.Count);
            pokemon2 = pokemonListModifiable[rng];
            pokemon2.UpdateLevel(pokemon1.Level);
            if (pokemon1.ScaledStats.Speed > pokemon2.ScaledStats.Speed)
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
