using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokemonFiche : MonoBehaviour
{
    [SerializeField] RawImage pokemonImage;
    [SerializeField] TextMeshProUGUI pokemonName;
    [SerializeField] TextMeshProUGUI pokemonLevel;
    [SerializeField] TextMeshProUGUI pokemonSpeed;
    [SerializeField] TextMeshProUGUI pokemonHealth;
    [SerializeField] TextMeshProUGUI pokemonAttack;
    [SerializeField] TextMeshProUGUI pokemonDefense;
    [SerializeField] TextMeshProUGUI pokemonSpAttack;
    [SerializeField] TextMeshProUGUI pokemonSpDefense;
    [SerializeField] Button prevButton;
    [SerializeField] Button nextButton;
    [SerializeField] Button ButtonMinus10;
    [SerializeField] Button ButtonMinus5;
    [SerializeField] Button ButtonMinus1;
    [SerializeField] Button ButtonPlus1;
    [SerializeField] Button ButtonPlus5;
    [SerializeField] Button ButtonPlus10;
    [SerializeField] Button Load10;
    [SerializeField] Button Load100;
    [SerializeField] Button LoadAll;
    [SerializeField] TMP_InputField gotoField;
    [SerializeField] TextMeshProUGUI gotoPlaceholder;
    public List<Pokemon> pokemonList;
    public Pokemon ActualPokemon { get; set; }
    int currentLevel = 1;
    int currentIndex = 0;

    void Awake()
    {
        ButtonMinus1.onClick.AddListener(delegate { RemoveLevel(1); });
        ButtonMinus5.onClick.AddListener(delegate { RemoveLevel(5); });
        ButtonMinus10.onClick.AddListener(delegate { RemoveLevel(10); });
        ButtonPlus1.onClick.AddListener(delegate { AddLevel(1); });
        ButtonPlus5.onClick.AddListener(delegate { AddLevel(5); });
        ButtonPlus10.onClick.AddListener(delegate { AddLevel(10); });
        Load10.onClick.AddListener(delegate { LoadPokemon(10); });
        Load100.onClick.AddListener(delegate { LoadPokemon(100); });
        LoadAll.onClick.AddListener(delegate { LoadPokemon(10000); });
    }

    public async void LoadPokemon(int howMany)
    {
        
        Load10.transform.parent.transform.parent.gameObject.SetActive(false);
        pokemonList = await PokemonDatabase.Instance.LoadPokemons(howMany);
        CheckPage();
        gotoPlaceholder.text = $"Enter ID (1 - {pokemonList.Count})";
        GetAndSetPokemon(0);
    }

    void GetAndSetPokemon(int index)
    {
        currentIndex = index;
        ActualPokemon = pokemonList[index];
        UpdateData();
    }

    void CheckPage()
    {
        if (currentIndex == 0)
        {
            prevButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(true);
        }
        else if (currentIndex == pokemonList.Count - 1)
        {
            nextButton.gameObject.SetActive(false);
            prevButton.gameObject.SetActive(true);
        }
        else
        {
            prevButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);
        }
    }

    public void GotoPage()
    {
        int index = int.Parse(gotoField.text) - 1;
        if (index >= 0 && index <= pokemonList.Count - 1)
        {
            gotoField.text = "";
            GetAndSetPokemon(index);
            CheckPage();
            UpdatePokemonLevel();
            UpdateData();
        }
    }

    public void PrevPage()
    {
        nextButton.gameObject.SetActive(true);
        if (currentIndex > 0)
        {
            GetAndSetPokemon(currentIndex - 1);
            CheckPage();
            UpdatePokemonLevel();
            UpdateData();
        }
    }

    public void NextPage()
    {
        prevButton.gameObject.SetActive(true);
        if (currentIndex < pokemonList.Count - 1)
        {
            GetAndSetPokemon(currentIndex + 1);
            CheckPage();
            UpdatePokemonLevel();
            UpdateData();
        }
    }
    public void UpdatePokemonLevel()
    {
        if(ActualPokemon.Level != currentLevel) ActualPokemon.UpdateLevel(currentLevel);
    }

    public void AddLevel(int level)
    {
        currentLevel = Mathf.Min(ActualPokemon.Level + level, 100);
        UpdatePokemonLevel();
        UpdateData();
    }

    public void RemoveLevel(int level)
    {
        currentLevel = Mathf.Max(ActualPokemon.Level - level, 1);
        UpdatePokemonLevel();
        UpdateData();
    }

    void UpdateData()
    {
        pokemonImage.texture = ActualPokemon.Texture;
        pokemonName.text = $"Name: {ActualPokemon.Name}";
        pokemonLevel.text = $"Level: {ActualPokemon.Level}";
        pokemonSpeed.text = $"Speed: {ActualPokemon.ScaledStats.Speed} ({ActualPokemon.BaseStats.Speed})";
        pokemonHealth.text = $"Health: {ActualPokemon.ScaledStats.Health} ({ActualPokemon.BaseStats.Health})";
        pokemonAttack.text = $"Attack: {ActualPokemon.ScaledStats.Attack} ({ActualPokemon.BaseStats.Attack})";
        pokemonDefense.text = $"Defense: {ActualPokemon.ScaledStats.Defense} ({ActualPokemon.BaseStats.Defense})";
        pokemonSpAttack.text = $"SpAttack: {ActualPokemon.ScaledStats.SpAttack} ({ActualPokemon.BaseStats.SpAttack})";
        pokemonSpDefense.text = $"SpDefense: {ActualPokemon.ScaledStats.SpDefense} ({ActualPokemon.BaseStats.SpDefense})";
    }
}
