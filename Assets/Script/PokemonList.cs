using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class PokemonList : MonoBehaviour
{
    [SerializeField] private PokemonFiche fiche;
    public static PokemonList Instance { get; private set; }
    [SerializeField] private List<Pokemon> unsortedPokemon = new();
    public List<Pokemon> SortedPokemon { get; private set; } = new();
    [SerializeField] GameObject loadingScreen;
    [SerializeField] TextMeshProUGUI loadingLogText;
    [SerializeField] Image pokeballLoading;
    int errorCount;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    /// <summary>
    /// Async function that will download a certain number of pokemon on https://pokeapi.co/api/v2
    /// </summary>
    /// <param name="howMany"How many pokemon should be loaded</param>
    /// <returns></returns>
    public async Task<List<Pokemon>> LoadPokemons(int howMany)
    {
        //Debug.Log("Entering LoadPokemons");
        pokeballLoading.gameObject.SetActive(true);
        if (SortedPokemon.Count > 0)
        {
            pokeballLoading.gameObject.SetActive(false);
            return SortedPokemon;
        }
        var pokemons = await DownloadLinks(howMany);
        pokeballLoading.transform.rotation = new Quaternion(0, 0, 0, 0);
        return pokemons;
    }

    async Task<List<Pokemon>> DownloadLinks(int howMany)
    {
        //Debug.Log("Entering DownloadLinks");
        using WebClient client = new();
        string json = await client.DownloadStringTaskAsync(new Uri("https://pokeapi.co/api/v2/pokemon?limit=" + howMany));
        PokemonJsonRoot pokemonJsonRoot = JsonConvert.DeserializeObject<PokemonJsonRoot>(json);
        float lerpedValue = 0f;
        for (int i = 0; i < pokemonJsonRoot.results.Length; i++)
        {
            await GetPokemon(pokemonJsonRoot.results[i].url);
            lerpedValue = i / (float)(pokemonJsonRoot.results.Length - 1);
            pokeballLoading.transform.Rotate(new Vector3(0, 0, 360 / (float)pokemonJsonRoot.results.Length));
        }
        SortedPokemon = unsortedPokemon.OrderBy(x => x.Id).ToList();
        loadingLogText.text = $"Loaded {pokemonJsonRoot.results.Length - errorCount}/{pokemonJsonRoot.results.Length} pokemons";
        StartCoroutine(FadeLoadingScreen());
        //Debug.Log("Exiting DownloadLinks");
        return SortedPokemon;
    }

    async Task GetPokemon(string url)
    {
        //Debug.Log("Entering GetPokemon");
        //Debug.Log("Entering GetPokemon");
        using WebClient client = new();
        try
        {
            string json = await client.DownloadStringTaskAsync(new Uri(url));
            PokemonJson tempPokemon = JsonConvert.DeserializeObject<PokemonJson>(json);
            loadingLogText.text = $"Downloaded {tempPokemon.name.FirstCharacterToUpper()}";
            if (tempPokemon.types.Length == 1) tempPokemon.types = new Types[2] { tempPokemon.types[0], new(2, new Type { name = "None" }) };
            Pokemon pokemon = new(tempPokemon.id,
                                  tempPokemon.name.FirstCharacterToUpper(),
                                  tempPokemon.stats[0].base_stat,
                                  tempPokemon.stats[1].base_stat,
                                  tempPokemon.stats[2].base_stat,
                                  tempPokemon.stats[3].base_stat,
                                  tempPokemon.stats[4].base_stat,
                                  tempPokemon.stats[5].base_stat,
                                  tempPokemon.sprites.front_default,
                                  Enum.Parse<Pokemon.PokemonType>(tempPokemon.types[0].type.name.FirstCharacterToUpper()),
                                  Enum.Parse<Pokemon.PokemonType>(tempPokemon.types[1].type.name.FirstCharacterToUpper()));
            unsortedPokemon.Add(pokemon);
            //Debug.Log("Exiting GetPokemon");
        }
        catch (WebException e)
        {
            errorCount++;
            Debug.LogException(e);
            loadingLogText.text = $"Error while downloading at {url}";
        }
    }

    IEnumerator FadeLoadingScreen()
    {
        float duration = 2f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            pokeballLoading.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), elapsedTime / duration);
            loadingLogText.alpha = Mathf.Lerp(1, 0, elapsedTime/duration);
            yield return null;
        } 
        loadingScreen.SetActive(false);
    }
}

internal class PokemonJsonRoot
{
    public PokemonJsonUrl[] results { get; set; }
}

public class PokemonJsonUrl
{
    public string url { get; set; }
}

public class PokemonJson
{
    public int id { get; set; }
    public string name { get; set; }
    public Stats[] stats { get; set; }
    public Types[] types { get; set; }
    public Sprites sprites { get; set; }
}

public class Sprites
{
    public string back_default { get; set; }
    public string back_female { get; set; }
    public string back_shiny { get; set; }
    public string back_shiny_female { get; set; }
    public string front_default { get; set; }
    public string front_female { get; set; }
    public string front_shiny { get; set; }
    public string front_shiny_female { get; set; }
}

public class Types
{
    public Types(int slot, Type type)
    {
        this.slot = slot;
        this.type = type;
    }
    public int slot { get; set; }
    public Type type { get; set; }
}

public class Type
{
    public string name { get; set; }
}

public class Stats
{
    public int base_stat { get; set; }
    public int effort { get; set; }
    public Stat stat { get; set; }
}
public class Stat
{
    public string name { get; set; }
}

#region OLD

//private string filePath;

//filePath = Application.persistentDataPath + "/pokedex.json";

/*public List<Pokemon> LoadPokemons()
{
    string loadData = File.ReadAllText(filePath);
    List<Pokemon> pokemons = new();
    RealPokemonData[] realPokemonDatas = JsonConvert.DeserializeObject<RealPokemonData[]>(loadData);
    foreach (RealPokemonData pokemon in realPokemonDatas)
    {
        if (pokemon.type.Length == 1) pokemon.type = new string[2] { pokemon.type[0], "None" };
        Pokemon newPokemon = new(pokemon.id, pokemon.name.french, pokemon.baseStats.health, pokemon.baseStats.speed, pokemon.baseStats.attack, pokemon.baseStats.defense, pokemon.baseStats.spAttack, pokemon.baseStats.spDefense, Enum.Parse<Pokemon.PokemonType>(pokemon.type[0]), Enum.Parse<Pokemon.PokemonType>(pokemon.type[1]));
        pokemons.Add(newPokemon);
    }
    return pokemons;
}*/


/*[Serializable]
public class RealPokemonData
{
    public int id;
    public Name name;
    public string[] type;
    public BaseStats baseStats;
}

[Serializable]
public class Name
{
    public string english;
    public string japanese;
    public string chinese;
    public string french;
}

[Serializable]
public class BaseStats
{
    public int health;
    public int attack;
    public int defense;
    public int spAttack;
    public int spDefense;
    public int speed;
}*/

#endregion