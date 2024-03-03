using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] public PokemonUI pokemonUI1;
    [SerializeField] public PokemonUI pokemonUI2;
    [SerializeField] GameObject levelGO;
    TMP_InputField levelInput;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject midUI;
    TextMeshProUGUI[] tmps;
    [SerializeField] TextMeshProUGUI tour;
    private int level = 1;
    public void PreFightVisual(Pokemon[] pokemons)
    {
        ResetAll();
        StartCoroutine(pokemonUI1.Init(pokemons[0]));
        StartCoroutine(pokemonUI2.Init(pokemons[1]));
        StartCoroutine(InitMidUI());
        startButton.SetActive(false);
        levelGO.SetActive(false);
    }

    private void ResetAll()
    {
    }

    public void FightVisual()
    {

    }

    public void PostFightVisual()
    {
        tour.text = "";
        startButton.SetActive(true);
        levelGO.SetActive(true);
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

    IEnumerator InitMidUI()
    {
        tmps = midUI.GetComponentsInChildren<TextMeshProUGUI>();
        yield return new WaitForSeconds(0.5f);
        foreach (var tmp in tmps)
        {
            switch (tmp.name)
            {
                case "Name":
                    tmp.enabled = true;
                    yield return new WaitForSeconds(0.5f);
                    break;

                case "Health":
                    tmp.enabled = true;
                    yield return new WaitForSeconds(0.5f);
                    break;

                case "Attack":
                    tmp.enabled = true;
                    yield return new WaitForSeconds(0.5f);
                    break;

                case "Defense":
                    tmp.enabled = true;
                    yield return new WaitForSeconds(0.5f);
                    break;

                case "Speed":
                    tmp.enabled = true;
                    yield return new WaitForSeconds(0.5f);
                    break;

                case "Types":
                    tmp.enabled = true;
                    yield return new WaitForSeconds(0.5f);
                    break;
            }
        }
        yield return new WaitForSeconds(1.5f);
        ResetMidUI();
    }

    void ResetMidUI()
    {
        tmps = midUI.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var tmp in tmps)
        {
            if (tmp == tour) continue;
            tmp.enabled = false;
        }
    }

    public void OnLevelChange()
    {
        if (levelInput == null) levelInput = levelGO.GetComponent<TMP_InputField>();
        level = int.Parse(levelInput.text);
    }

    public int GetLevel()
    {
        return level;
    }
}
