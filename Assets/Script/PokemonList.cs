using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
public class PokemonList
{
    readonly string filePath = Application.persistentDataPath + "/pokedex.json";
    public List<Pokemon> LoadPokemons()
    {
        string loadData = File.ReadAllText(filePath);
        List<Pokemon> pokemons = new();
        RealPokemonData[] realPokemonDatas = JsonConvert.DeserializeObject<RealPokemonData[]>(loadData);
        foreach (RealPokemonData pokemon in realPokemonDatas)
        {
            if (pokemon.type.Length == 1) pokemon.type = new string[2] { pokemon.type[0], "None" };
            Pokemon newPokemon = new(pokemon.id, pokemon.name.french, pokemon.baseStats.health, pokemon.baseStats.attack, pokemon.baseStats.defense, pokemon.baseStats.speed, Enum.Parse<Pokemon.PokemonType>(pokemon.type[0]), Enum.Parse<Pokemon.PokemonType>(pokemon.type[1]));
            pokemons.Add(newPokemon);
        }
        return pokemons;
    }
}

[Serializable]
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
}


//new(Resources.Load<Texture>("PokemonSprite/XXID"), ID, "Name", BaseHealth, Attack, Defense, Speed, Pokemon.PokemonType.Type1, [Pokemon.PokemonType.Type2])
/*public readonly static List<Pokemon> pokemons = new()
{
new(Resources.Load<Texture>("PokemonSprite/0001"), 1, "Bulbizarre", 45, 45, 49, 45, Pokemon.PokemonType.Grass, Pokemon.PokemonType.Poison),
new(Resources.Load<Texture>("PokemonSprite/0004"), 4, "Salamèche", 39, 52, 43, 65, Pokemon.PokemonType.Fire),
new(Resources.Load<Texture>("PokemonSprite/0007"), 7, "Carapuce", 44, 48, 65, 43, Pokemon.PokemonType.Water),
new(Resources.Load<Texture>("PokemonSprite/0025"), 25, "Pikachu", 35, 55, 40, 90, Pokemon.PokemonType.Electrik)
};*/