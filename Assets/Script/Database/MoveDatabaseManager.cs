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

public class MoveDatabaseManager : MonoBehaviour
{
    public static MoveDatabaseManager Instance { get; private set; }
    [field: SerializeField] public List<Move> SortedMove { get; private set; } = new();
    [SerializeField] private List<Move> unsortedMove = new();
    [SerializeField] public MoveDatabase movedb;
    [SerializeField] TextMeshProUGUI loadingLogText;
    int errorCount;
    private async void Start()
    {
        //.moves.Clear(); uncomment to clear (UI will be froze during DL, unlucky)
        movedb.moves = await LoadMoves(919); //919 ignore weird moves (shadow stuff from other game)
    }
    public async Task<List<Move>> LoadMoves(int howMany)
    {
        //Debug.Log("Entering LoadMoves");
        var moves = await DownloadLinks(howMany);
        return moves;
    }

    async Task<List<Move>> DownloadLinks(int howMany)
    {
        //Debug.Log("Entering DownloadLinks");
        using WebClient client = new();
        string json = await client.DownloadStringTaskAsync(new Uri("https://pokeapi.co/api/v2/move?limit=" + howMany));
        MoveJsonRoot moveJsonRoot = JsonConvert.DeserializeObject<MoveJsonRoot>(json);
        List<Task> tasks = new();
        if (moveJsonRoot.results.Length == movedb.moves.Count) //if number of found moves = already loaded moves, then skip (always true as you always download 919 moves)
        {
            Debug.Log("Same number of moves, skipping download");
            return movedb.moves;
        }
        for (int i = 0; i < moveJsonRoot.results.Length; i++)
        {
            tasks.Add(Task.Run(() => GetMove(moveJsonRoot.results[i].url)));
            Debug.Log(i);
        }
        Task.WaitAll(tasks.ToArray());
        Debug.Log("All tasks done, sorting..");
        SortedMove = unsortedMove.OrderBy(x => x.Id).ToList();
        Debug.Log("Sorted!");
        //Debug.Log("Exiting DownloadLinks");
        return SortedMove;
    }

    async Task GetMove(string url)
    {
        //Debug.Log("Entering GetMove");
        //Debug.Log($"Started: Thread {Thread.CurrentThread.ManagedThreadId} - Id {url.Split('/')[6]}");
        using WebClient client = new();
        try
        {
            string json = await client.DownloadStringTaskAsync(new Uri(url));
            MoveJson tempMove = JsonConvert.DeserializeObject<MoveJson>(json);
            tempMove.accuracy ??= 100;
            tempMove.power ??= 0;
            if (tempMove.type.name == "status") return;
            //loadingLogText.text = $"Downloaded {tempMove.name.FirstCharacterToUpper()}";
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
            //Debug.Log($"Ended: Thread {Thread.CurrentThread.ManagedThreadId} - Id {url.Split('/')[6]}");
            return;
            //Debug.Log("Exiting GetMove");
        }
        catch (WebException e)
        {
            errorCount++;
            Debug.LogException(e);
            loadingLogText.text = $"Error while downloading at {url}";
            return;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return;
        }
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
