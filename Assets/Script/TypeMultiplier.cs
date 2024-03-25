using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TypeMultiplier
{
    // STRUCT OR MATRIX
    //EFFECTIVEATTACK
    readonly Dictionary<Pokemon.PokemonType, Pokemon.PokemonType[]> effectiveAttack = new()
    {
        {Pokemon.PokemonType.Normal, new Pokemon.PokemonType[]{}},
        {Pokemon.PokemonType.Fire, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Grass, Pokemon.PokemonType.Ice, Pokemon.PokemonType.Bug, Pokemon.PokemonType.Steel}},
        {Pokemon.PokemonType.Water, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Fire, Pokemon.PokemonType.Ground, Pokemon.PokemonType.Rock}},
        {Pokemon.PokemonType.Grass, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Water, Pokemon.PokemonType.Ground, Pokemon.PokemonType.Rock}},
        {Pokemon.PokemonType.Electric, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Water, Pokemon.PokemonType.Flying}},
        {Pokemon.PokemonType.Ice, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Electric, Pokemon.PokemonType.Ground, Pokemon.PokemonType.Flying, Pokemon.PokemonType.Dragon}},
        {Pokemon.PokemonType.Fighting, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Normal, Pokemon.PokemonType.Ice, Pokemon.PokemonType.Rock, Pokemon.PokemonType.Dark, Pokemon.PokemonType.Steel}},
        {Pokemon.PokemonType.Poison, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Grass}},
        {Pokemon.PokemonType.Ground, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Fire, Pokemon.PokemonType.Electric, Pokemon.PokemonType.Poison, Pokemon.PokemonType.Rock, Pokemon.PokemonType.Steel}},
        {Pokemon.PokemonType.Flying, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Grass, Pokemon.PokemonType.Fighting, Pokemon.PokemonType.Bug}},
        {Pokemon.PokemonType.Psychic, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Fighting, Pokemon.PokemonType.Poison}},
        {Pokemon.PokemonType.Bug, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Grass, Pokemon.PokemonType.Psychic, Pokemon.PokemonType.Dark}},
        {Pokemon.PokemonType.Rock, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Fire, Pokemon.PokemonType.Ice, Pokemon.PokemonType.Flying, Pokemon.PokemonType.Bug}},
        {Pokemon.PokemonType.Ghost, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Psychic, Pokemon.PokemonType.Ghost}},
        {Pokemon.PokemonType.Dragon, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Dragon}},
        {Pokemon.PokemonType.Dark, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Psychic, Pokemon.PokemonType.Ghost}},
        {Pokemon.PokemonType.Steel, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Ice, Pokemon.PokemonType.Rock}},
        {Pokemon.PokemonType.Fairy, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Fighting, Pokemon.PokemonType.Dark, Pokemon.PokemonType.Dragon}},
    };

    //WEAKATTACK
    readonly Dictionary<Pokemon.PokemonType, Pokemon.PokemonType[]> weakAttack = new()
    {
        {Pokemon.PokemonType.Normal, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Rock, Pokemon.PokemonType.Steel}}, //NORMAL > DARK = 0
        {Pokemon.PokemonType.Fire, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Fire, Pokemon.PokemonType.Water, Pokemon.PokemonType.Rock, Pokemon.PokemonType.Dragon}},
        {Pokemon.PokemonType.Water, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Water, Pokemon.PokemonType.Grass, Pokemon.PokemonType.Dragon}},
        {Pokemon.PokemonType.Grass, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Fire, Pokemon.PokemonType.Grass, Pokemon.PokemonType.Poison, Pokemon.PokemonType.Flying, Pokemon.PokemonType.Bug, Pokemon.PokemonType.Dragon, Pokemon.PokemonType.Steel}},
        {Pokemon.PokemonType.Electric, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Grass, Pokemon.PokemonType.Electric, Pokemon.PokemonType.Dragon}}, //ELECTRIK > GROUND = 0
        {Pokemon.PokemonType.Ice, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Fire, Pokemon.PokemonType.Water, Pokemon.PokemonType.Ice, Pokemon.PokemonType.Steel}},
        {Pokemon.PokemonType.Fighting, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Poison, Pokemon.PokemonType.Ground, Pokemon.PokemonType.Psychic, Pokemon.PokemonType.Poison}}, //FIGHTING > GHOST = 0  
        {Pokemon.PokemonType.Poison, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Poison, Pokemon.PokemonType.Ground, Pokemon.PokemonType.Rock, Pokemon.PokemonType.Ghost}}, //POISON > STEEL = 0
        {Pokemon.PokemonType.Ground, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Grass, Pokemon.PokemonType.Bug}}, //GROUND > FLYING = 0
        {Pokemon.PokemonType.Flying, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Electric, Pokemon.PokemonType.Rock, Pokemon.PokemonType.Steel}},
        {Pokemon.PokemonType.Psychic, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Psychic, Pokemon.PokemonType.Steel}}, //PSYCHIC > DARK = 0
        {Pokemon.PokemonType.Bug, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Fire, Pokemon.PokemonType.Fighting, Pokemon.PokemonType.Poison, Pokemon.PokemonType.Flying, Pokemon.PokemonType.Ghost, Pokemon.PokemonType.Steel}},
        {Pokemon.PokemonType.Rock, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Fighting, Pokemon.PokemonType.Flying, Pokemon.PokemonType.Steel}},
        {Pokemon.PokemonType.Ghost, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Dark, Pokemon.PokemonType.Steel}}, //GHOST > NORMAL = 0
        {Pokemon.PokemonType.Dragon, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Steel}},
        {Pokemon.PokemonType.Dark, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Fighting, Pokemon.PokemonType.Dark, Pokemon.PokemonType.Steel}},
        {Pokemon.PokemonType.Steel, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Fire, Pokemon.PokemonType.Water, Pokemon.PokemonType.Steel}},
        {Pokemon.PokemonType.Fairy, new Pokemon.PokemonType[]{ Pokemon.PokemonType.Fire, Pokemon.PokemonType.Steel, Pokemon.PokemonType.Poison}},
    };

    public float DamageMultiplier(Pokemon.PokemonType sourceType, Pokemon.PokemonType[] targetType)
    {
        float damageMultiplier = 1f;
        if (sourceType != Pokemon.PokemonType.None)
        {
            foreach (Pokemon.PokemonType type in targetType)
            {
                if (effectiveAttack[sourceType].Contains(type))
                {
                    damageMultiplier *= 2f;
                }
            }
            foreach (Pokemon.PokemonType type in targetType)
            {
                if (weakAttack[sourceType].Contains(type))
                {
                    damageMultiplier *= 0.5f;
                }
            }
        }
        return damageMultiplier;
    }
}
