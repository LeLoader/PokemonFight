using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TMPro;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using System.Reflection;

public class MoveDatabaseManager : MonoBehaviour, ILogger
{
    [SerializeField] public MoveDatabase movedb;
    public static MoveDatabaseManager Instance { get; private set; }
    [field: SerializeField] public List<Move> SortedMove { get; set; } = new();
    [SerializeField] private List<Move> unsortedMove = new();
    [SerializeField] TextMeshProUGUI loadingLogText;
    string log;
    [SerializeField] int errorCount;

    private void Awake()
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
        if (loadingLogText.text != log || log != "") loadingLogText.text = log;
    }

    public async Task<List<Move>> LoadMoves(int howMany)
    {
        //Debug.Log("Entering LoadMoves");

        var moves = await DownloadLinks(howMany);
        movedb.moves = moves;
        return moves;
    }

    async Task<List<Move>> DownloadLinks(int howMany)
    {
        //Debug.Log("Entering DownloadLinks");
        using WebClient client = new();
        string json = await client.DownloadStringTaskAsync(new Uri("https://pokeapi.co/api/v2/move?limit=" + howMany));
        MoveJsonRoot moveJsonRoot = JsonConvert.DeserializeObject<MoveJsonRoot>(json);
        Debug.Log($"Found {SortedMove.Count} moves locally, and found {moveJsonRoot.results.Length} moves online");
        if (moveJsonRoot.results.Length == movedb.moves.Count) //if number of found moves = already loaded moves, then skip (always true as you always download 919 moves)
        {
            Debug.Log("Same number of moves, skipping download");
            return SortedMove;
        }
        for (int i = 0; i < moveJsonRoot.results.Length; i++)
        {
            await GetMove(moveJsonRoot.results[i].url);
        }
        log = $"Loaded {moveJsonRoot.results.Length - errorCount}/{moveJsonRoot.results.Length} moves";
        Debug.Log("All tasks done, sorting..");
        SortedMove = unsortedMove.OrderBy(x => x.Id).ToList();
        Debug.Log("Sorted!");
        //Debug.Log("Exiting DownloadLinks");
        return SortedMove;
    }

    async Task GetMove(string url)
    {
        //Debug.Log("Entering GetMove");
        using WebClient client = new();
        try
        {
            string json = await client.DownloadStringTaskAsync(new Uri(url));
            MoveJson tempMove = JsonConvert.DeserializeObject<MoveJson>(json);
            log = $"Downloaded {tempMove.name.FirstCharacterToUpper()}";
            tempMove.accuracy ??= 100;
            tempMove.power ??= 0;
            if (tempMove.damage_class.name == "status") //REMOVE STATUS MOVE SINCE POWER 0 IS USELESS IS THIS GAMEMODE
            {
                Debug.Log(tempMove.name + " " + tempMove.damage_class.name);
                return;
            }
            if (tempMove.power == 0) return; //REMOVE POWER 0 SINCE IT'S USELESS IS THIS GAMEMODE
            if (tempMove.target.name != "selected-pokemon") return; //REMOVE MOVE THAT TARGET SOMETHING ELSE THAN ENEMY (NOT SURE)
            Move move = new(tempMove.id,
                            tempMove.name,
                            tempMove.accuracy,
                            tempMove.power,
                            Enum.Parse<MoveType>(tempMove.damage_class.name.FirstCharacterToUpper()),
                            Enum.Parse<ElementalType>(tempMove.type.name.FirstCharacterToUpper())
                            );
            unsortedMove.Add(move);
            move.learnedByPokemons = new string[tempMove.learned_by_pokemon.Count];
            for (int i = 0; i < tempMove.learned_by_pokemon.Count; i++)
            {
                move.learnedByPokemons[i] = tempMove.learned_by_pokemon[i].name;
            }
            //Debug.Log("Exiting GetMove");
            return;
        }
        catch (WebException e)
        {
            errorCount++;
            Debug.LogException(e);
            log = $"Error while downloading at {url}";
            return;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return;
        }
    }

    public List<Move> GetMoveForPokemon(Pokemon pokemon)
    {
        var query = movedb.moves.Where((move) => move.learnedByPokemons.Contains(pokemon.Name.ToLower()));
        return query.ToList();
    }
}

internal class MoveJsonRoot
{
    public MoveJsonUrl[] results { get; set; }
}

public class MoveJsonUrl
{
    public string url { get; set; }
}
public class Damage_class
{
    public string name { get; set; }
}

public class Target
{
    public string name { get; set; }

}
public class ElementalTypeJson
{
    public string name { get; set; }

}
public class Learned_by_pokemon
{
    public string name { get; set; }
}
public class MoveJson
{
    public int id { get; set; }
    public string name { get; set; }
    public int? accuracy { get; set; }
    public int? power { get; set; }
    public Damage_class damage_class { get; set; }
    public Target target { get; set; }
    public ElementalTypeJson type { get; set; }
    public IList<Learned_by_pokemon> learned_by_pokemon { get; set; }
}
