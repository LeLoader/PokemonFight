using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] public PokemonUI pokemonUI1;
    [SerializeField] public PokemonUI pokemonUI2;
    [SerializeField] TextMeshProUGUI levelInput;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject midUI;
    [SerializeField] TextMeshProUGUI tour;
    [SerializeField] GameObject attacks;
    [Range(1f, 100f)]
    private int level = 1;
    public void PreFightVisual(Pokemon[] pokemons)
    {
        ResetAll();
        StartCoroutine(pokemonUI1.Init(pokemons[0]));
        StartCoroutine(pokemonUI2.Init(pokemons[1]));
        startButton.SetActive(false);
        attacks.SetActive(true);
    }

    private void ResetAll()
    {
        pokemonUI1.ResetAll();
        pokemonUI2.ResetAll();
    }

    public void FightVisual()
    {

    }

    public void PostFightVisual()
    {
        ResetAll();
        tour.text = "";
        startButton.SetActive(true);
        attacks.SetActive(false);
    }

    public void IncrementTurn(int turn)
    {
        tour.text = "TOUR: " + turn;
    }

    public void DamageAnimation(Pokemon pokemon)
    {
        if (pokemon == pokemonUI1.pokemon) pokemonUI1.DamagedAnimation();
        else if (pokemon == pokemonUI2.pokemon) pokemonUI2.DamagedAnimation();
    }

    public int GetLevel()
    {
        return level;
    }
}
