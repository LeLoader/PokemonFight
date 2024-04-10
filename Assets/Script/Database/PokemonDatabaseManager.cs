using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Unity.VisualScripting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using UnityEngine.Networking;

public class PokemonDatabaseManager : MonoBehaviour, ILogger
{
    public static PokemonDatabaseManager Instance { get; private set; }
    [SerializeField] public PokemonDatabase pokemondb;
    public List<Pokemon> unsortedPokemon = new();
    [field: SerializeField] public List<Pokemon> SortedPokemon { get; set; } = new();
    [SerializeField] GameObject loadingScreen;
    [SerializeField] TextMeshProUGUI loadingLogText;
    string log;
    [SerializeField] Image pokeballLoading;
    [SerializeField] int errorCount;
    [SerializeField] public PokemonFight pokemonFight;

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

    private void Update()
    {
        if (loadingLogText.text != log || log != "") loadingLogText.text = log; ;
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
        var pokemons = await DownloadLinks(howMany);
        pokemondb.pokemons = pokemons;
        pokeballLoading.transform.rotation = new Quaternion(0, 0, 0, 0);
        return pokemons;
    }

    async Task<List<Pokemon>> DownloadLinks(int howMany)
    {
        //Debug.Log("Entering DownloadLinks");
        using WebClient client = new();
        string json = await client.DownloadStringTaskAsync(new Uri("https://pokeapi.co/api/v2/pokemon?limit=" + howMany));
        PokemonJsonRoot pokemonJsonRoot = JsonConvert.DeserializeObject<PokemonJsonRoot>(json);
        Debug.Log($"Found {SortedPokemon.Count} pokemons locally, and found {pokemonJsonRoot.results.Length} pokemons online");
        if (pokemonJsonRoot.results.Length == pokemondb.pokemons.Count) //if number of found moves = already loaded moves, then skip
        {
            Debug.Log("Same number of pokemons, skipping download");
            StartCoroutine(FadeLoadingScreen());
            return SortedPokemon;
        }
        for (int i = 0; i < pokemonJsonRoot.results.Length; i++)
        {
            await GetPokemon(pokemonJsonRoot.results[i].url);
        }
        log = $"Loaded {pokemonJsonRoot.results.Length - errorCount}/{pokemonJsonRoot.results.Length} pokemons";
        Debug.Log("All tasks done, sorting..");
        SortedPokemon = unsortedPokemon.OrderBy(x => x.Id).ToList();
        Debug.Log("Sorted!");
        StartCoroutine(FadeLoadingScreen());
        //Debug.Log("Exiting DownloadLinks");
        return SortedPokemon;
    }

    async Task GetPokemon(string url)
    {
        //Debug.Log("Entering GetPokemon");     
        using WebClient client = new();
        try
        {
            string json = await client.DownloadStringTaskAsync(new Uri(url));
            PokemonJson tempPokemon = JsonConvert.DeserializeObject<PokemonJson>(json);
            log = $"Downloaded {tempPokemon.name.FirstCharacterToUpper()}";
            if (tempPokemon.types.Length == 1) tempPokemon.types = new Types[2] { tempPokemon.types[0], new(2, new Type { name = "None" }) };
            Pokemon pokemon = new(tempPokemon.id,
                                  tempPokemon.name.FirstCharacterToUpper(),
                                  tempPokemon.stats[0].base_stat,
                                  tempPokemon.stats[1].base_stat,
                                  tempPokemon.stats[2].base_stat,
                                  tempPokemon.stats[3].base_stat,
                                  tempPokemon.stats[4].base_stat,
                                  tempPokemon.stats[5].base_stat,
                                  Enum.Parse<ElementalType>(tempPokemon.types[0].type.name.FirstCharacterToUpper()),
                                  Enum.Parse<ElementalType>(tempPokemon.types[1].type.name.FirstCharacterToUpper()));
            unsortedPokemon.Add(pokemon);
            //Debug.Log("Exiting GetPokemon");
            return;
        }
        catch (WebException e)
        {
            errorCount++;
            Debug.LogException(e);
            log = $"Error while downloading at {url}";
        }
        catch (Exception e)
        {
            errorCount++;
            Debug.LogException(e);
            return;
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
            loadingLogText.alpha = Mathf.Lerp(1, 0, elapsedTime / duration);
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
