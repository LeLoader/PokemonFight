using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Pokemon
{
    Transform transform;
    int id;
    int level = 1;
    string name;
    readonly int baseHealth;
    int health;
    int attack;
    int defense;
    int speed;
    public enum PokemonType {None, Normal, Fire, Water, Electric, Grass, Ice, Fighting, Poison, Ground, Flying, Psychic, Bug, Rock, Ghost, Dragon, Dark, Steel, Fairy, Stellar};
    PokemonType type1;
    PokemonType type2;
    Texture texture;

    public Pokemon(int id, string name, int baseHealth, int attack, int defense, int speed, PokemonType type1, PokemonType type2 = PokemonType.None)
    {
        this.id = id;
        this.name = name;
        this.baseHealth = baseHealth;
        this.attack = attack;
        this.defense = defense;
        this.speed = speed;
        this.type1 = type1;
        this.type2 = type2;
        texture = Resources.Load<Texture>("PokemonSprite/" + id.ToString("D3") + "MS");
        if (Resources.Load<Texture>("PokemonSprite/" + id.ToString("D3") + "MS") == null) texture = Resources.Load<Texture>("PokemonSprite/000MS");
    }

    public void PostInit(int level)
    {
        this.level = level;
        health = GetMaxHealth();
        attack = ScaleToLevel(attack);
        defense = ScaleToLevel(defense);
        speed = ScaleToLevel(speed);
    }

    public Texture GetTexture() 
    { 
        return texture; 
    }

    public string GetName()
    {
        return name;
    }

    public int GetBaseHealth()
    {
        return baseHealth;
    }

    public int GetMaxHealth()
    {
        return (int)(Mathf.Floor(0.01f * (2f * baseHealth) * level) + level + 10);
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetAttack()
    {
        return attack;
    }

    public int GetDefense()
    {
        return defense;
    }

    public int GetSpeed()
    {
        return speed;
    }

    public new PokemonType[] GetType()
    {
        return new PokemonType[] {type1, type2};
    }

    private int ScaleToLevel(int stat)
    {
        return (int)Mathf.Floor(0.01f * (2f * stat) * level) + 5;
    }

    public void Attack(Pokemon target)
    {
        TypeMultiplier typeMultiplierClass = new();
        int critMultiplier = CriticalMultiplier(speed / 2f / 256f);
        float typeMultiplier = typeMultiplierClass.DamageMultiplier(GetType()[0], target.GetType()) * typeMultiplierClass.DamageMultiplier(GetType()[1], target.GetType());
        float lostHP = (((2 * level * critMultiplier / 5f + 2f) * Random.Range(40, Mathf.Min(40 + level + 1, 100)) * attack / defense / 50f + 2f) * typeMultiplier * (Random.Range(217, 256) / 255f));
        Debug.LogWarning("HP LOST: " + lostHP);
        target.health -= (int)lostHP;
        Debug.Log($"{name} attaque {target.GetName()}");
        if(lostHP > 0)
        {
            switch (typeMultiplier)
            {
                case 0:
                    Debug.Log($"Pas d'effet sur {target.GetName()}");
                    break;
                case < 1:
                    Debug.Log($"Ce n'est pas très efficace...");
                    break;
                case >= 2:
                    Debug.Log($"C'est très efficace!");
                    break;
            }
            if (target.health > 0) Debug.Log($"{target.GetName()} a desormait {target.GetHealth()} HP!");
            else Debug.Log($"{target.GetName()} est K.O");
        }
        else
        {
            Debug.Log($"L'attaque n'a d'effet sur {target.GetName()}");
        }
    }

    private int CriticalMultiplier(float chance)
    {
        var rng = Random.Range(0, 256) / 256f;
        if (rng < chance) return 2;
        else return 1;
    }
}
