using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PokemonFight : MonoBehaviour
{
    List<Pokemon> pokemonList;
    Pokemon firstPokemon;
    Pokemon secondPokemon;
    Pokemon playerPokemon;
    [SerializeField] UIManager uiManager;
    [SerializeField] GameObject fichePokemon;
    [SerializeField] private List<AttackButton> attackButton;
    bool playerClicked = false;
    Move currentMove;

    public void StartFight()
    {
        StartCoroutine(PreFight());
    }

    private IEnumerator PreFight()
    {
        fichePokemon.SetActive(false);
        pokemonList = PokemonDatabaseManager.Instance.SortedPokemon;
        ChooseTwoPokemons();
        uiManager.PreFightVisual(new Pokemon[] { firstPokemon, secondPokemon });
        yield return new WaitForSeconds(5);
        StartCoroutine(Fight());
    }

    public void OnAttackButtonDown(Move move)
    {
        currentMove = move;
        playerClicked = true;
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
                if (firstPokemon == playerPokemon)
                {
                    yield return new WaitUntil(() => playerClicked == true);
                    firstPokemon.AttackPokemon(secondPokemon, currentMove);
                }
                else
                {
                    yield return new WaitForSeconds(2);
                    firstPokemon.AttackPokemon(secondPokemon, firstPokemon.moves[Random.Range(0, firstPokemon.moves.Count)]);
                }
                uiManager.IncrementTurn((turn + 1) / 2);
                uiManager.DamageAnimation(secondPokemon);
                                uiManager.pokemonUI2.OnHealthChange(secondPokemon.CurrentHealth);
                playerClicked = false;
                currentMove = null;
            }
            else if (turn % 2 == 0) //SECOND POKEMON
            {
                uiManager.IncrementTurn((turn + 1 ) / 2);
                if (secondPokemon == playerPokemon)
                {
                    yield return new WaitUntil(() => playerClicked == true);
                    secondPokemon.AttackPokemon(firstPokemon, currentMove);
                }
                else
                {
                    yield return new WaitForSeconds(2);
                    secondPokemon.AttackPokemon(firstPokemon, secondPokemon.moves[Random.Range(0, firstPokemon.moves.Count)]);
                }
                uiManager.DamageAnimation(firstPokemon);
                uiManager.pokemonUI1.OnHealthChange(firstPokemon.CurrentHealth);
                playerClicked = false;
                currentMove = null;
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

    private void ChooseTwoPokemons()
    {
        List<Pokemon> pokemonListModifiable = new(pokemonList);
        Pokemon pokemon1 = fichePokemon.GetComponent<PokemonFiche>().ActualPokemon;
        playerPokemon = pokemon1;
        pokemon1.moves = FourRandomMoves(MoveDatabaseManager.Instance.GetMoveForPokemon(pokemon1));

        for (int i = 0; i < attackButton.Count; i++)
        {
            attackButton[i].Init(pokemon1.moves[i]);
        }
        Pokemon pokemon2;
        pokemonListModifiable.Remove(pokemon1);
        if(pokemonListModifiable.Count != 0)
        {
            int rng = Random.Range(0, pokemonListModifiable.Count);
            pokemon2 = pokemonListModifiable[rng];
            pokemon2.moves = FourRandomMoves(MoveDatabaseManager.Instance.GetMoveForPokemon(pokemon1));
            pokemon2.UpdateLevel(pokemon1.Level);
            Debug.Log(pokemon1.Id);
            Debug.Log(pokemon2.Id);
            Debug.Log(pokemon2.Texture);
            if (pokemon1.ScaledStats.Speed >= pokemon2.ScaledStats.Speed)
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

    List<Move> FourRandomMoves(List<Move> allMoves)
    {
        switch(allMoves.Count)
        {
            case 0:
                return new List<Move>();
            case <= 4:
                return allMoves;
            case > 4:
                List<Move> newMoves = new();
                for (int i = 0; i < 4; i++)
                {
                    var rng = Random.Range(0, allMoves.Count);
                    newMoves.Add(allMoves[rng]);
                    allMoves.RemoveAt(rng);
                }
                return newMoves;
        }
    }
}
