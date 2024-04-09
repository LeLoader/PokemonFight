using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PokemonDatabase", menuName = "Database/Create Pokemon Database")]
public class PokemonDatabase : ScriptableObject
{
    public List<Pokemon> pokemons;
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