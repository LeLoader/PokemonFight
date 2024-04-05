using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Start : MonoBehaviour
{
    [field: SerializeField] public List<Move> SortedMove { get; private set; } = new();
    [SerializeField] private List<Move> unsortedMove = new();
    [SerializeField] MoveDatabase movedb;
    int errorCount;
    private async void Awake()
    {
        movedb.moves = await LoadMoves(1000);
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
        for (int i = 0; i < moveJsonRoot.results.Length; i++)
        {
            await GetMove(moveJsonRoot.results[i].url);
        }
        SortedMove = unsortedMove.OrderBy(x => x.Id).ToList();
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
            Debug.Log(json);
            MoveJson tempMove = JsonConvert.DeserializeObject<MoveJson>(json);
            tempMove.accuracy ??= 100;
            tempMove.power ??= 0;
            //loadingLogText.text = $"Downloaded {tempMove.name.FirstCharacterToUpper()}";
            Move move = new(tempMove.id,
                            tempMove.name,
                            tempMove.accuracy,
                            tempMove.power,
                            Enum.Parse<MoveType>(tempMove.damage_class.name.FirstCharacterToUpper()),
                            Enum.Parse<ElementalType>(tempMove.type.name.FirstCharacterToUpper())
                            ); ;
            unsortedMove.Add(move);
            //Debug.Log("Exiting GetMove");
        }
        catch (WebException e)
        {
            errorCount++;
            Debug.LogException(e);
            //loadingLogText.text = $"Error while downloading at {url}";
        }
        catch (Exception e)
        {
            Debug.LogException(e);
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
