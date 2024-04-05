using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Pokemon : BaseData
{
    public int Level { get; set; } = 1;
    public int CurrentHealth { get; set; }
    public Stats BaseStats { get; set; }
    public Stats ScaledStats { get; set; }
    ElementalType type1;
    ElementalType type2;
    public string textureLink;
    public Texture Texture { get; set; }
    public List<Move> moves = new();


    [Serializable]
    public struct Stats
    {
        public Stats(int health, int attack, int defense, int spAttack, int spDefense, int speed)
        {
            Health = health;
            Attack = attack;
            Defense = defense;
            SpAttack = spAttack;
            SpDefense = spDefense;
            Speed = speed;
        }
        public int Health { get; set;  }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpAttack { get; set; }
        public int SpDefense { get; set; }
        public int Speed { get; set; }
    }

    public Pokemon(int id, string name, int baseHealth, int attack, int defense, int spAttack, int spDefense, int speed, string texture, ElementalType type1, ElementalType type2 = ElementalType.None) : base (id, name)
    {
        Id = id;
        Name = name;
        BaseStats = new()
        {
            Health = baseHealth,
            Attack = attack,
            Defense = defense,
            SpAttack = spAttack,
            SpDefense = spDefense,
            Speed = speed,
        };
        ScaledStats = new()
        {
            Health = ScaleHealthToLevel(baseHealth),
            Attack = ScaleToLevel(attack),
            Defense = ScaleToLevel(defense),
            SpAttack = ScaleToLevel(BaseStats.SpAttack),
            SpDefense = ScaleToLevel(BaseStats.SpDefense),
            Speed = ScaleToLevel(speed),
        };
        CurrentHealth = ScaledStats.Health;
        this.type1 = type1;
        this.type2 = type2;
        textureLink = texture;
        Texture = Resources.Load<Texture>("PokemonSprite/" + id.ToString("D3") + "MS");
        if (Resources.Load<Texture>("PokemonSprite/" + id.ToString("D3") + "MS") == null) Texture = Resources.Load<Texture>("PokemonSprite/000MS");
    }

    public new ElementalType[] GetType()
    {
        return new ElementalType[] {type1, type2};
    }

    private int ScaleHealthToLevel(int baseHealth)
    {
        return (int)(Mathf.Floor(0.01f * (2f * baseHealth) * Level) + Level + 10);
    }

    private int ScaleToLevel(int stat)
    {
        return (int)Mathf.Floor(0.01f * (2f * stat) * Level) + 5;
    }

    public void UpdateLevel(int level)
    {
        Level = level;
        ScaledStats = RescaleStats();
        CurrentHealth = ScaledStats.Health;
    }

    private Stats RescaleStats()
    {
        return new Stats
        {
            Health = ScaleHealthToLevel(BaseStats.Health),
            Attack = ScaleToLevel(BaseStats.Attack),
            Defense = ScaleToLevel(BaseStats.Defense),
            SpAttack = ScaleToLevel(BaseStats.SpAttack),
            SpDefense = ScaleToLevel(BaseStats.SpDefense),
            Speed = ScaleToLevel(BaseStats.Speed),
        };
    }

    public void AttackPokemon(Pokemon target)
    {
        TypeMultiplier typeMultiplierClass = new();
        int critMultiplier = CriticalMultiplier(ScaledStats.Speed / 2f / 256f);
        float typeMultiplier = typeMultiplierClass.DamageMultiplier(GetType()[0], target.GetType()) * typeMultiplierClass.DamageMultiplier(GetType()[1], target.GetType());
        float lostHP = ((2 * Level * critMultiplier / 5f + 2f) * Random.Range(40, Mathf.Min(40 + Level + 1, 100)) * ScaledStats.Attack / ScaledStats.Defense / 50f + 2f) * typeMultiplier * (Random.Range(217, 256) / 255f);
        Debug.LogWarning($"PREHP: {target.CurrentHealth}/{target.ScaledStats.Health}");
        Debug.LogWarning("HP LOST: " + lostHP);
        target.CurrentHealth -= (int)lostHP;
        Debug.LogWarning($"POSTHP: {target.CurrentHealth}/{target.ScaledStats.Health}");
        Debug.Log($"{Name} attacks {target.Name}");
        if(lostHP >= 1)
        {
            switch (typeMultiplier)
            {
                case 0:
                    Debug.Log($"This has no effect on {target.Name}");
                    break;
                case < 1:
                    Debug.Log($"It's not very effective...");
                    break;
                case >= 2:
                    Debug.Log($"It's super effective!");
                    break;
            }
            if (target.CurrentHealth > 0) Debug.Log($"{target.Name} has now {target.CurrentHealth} HP!");
            else Debug.Log($"{target.Name} is K.O!");
        }
        else
        {
            Debug.Log($"The attack is to weak to damage to {target.Name}");
        }
    }

    private int CriticalMultiplier(float chance)
    {
        var rng = Random.Range(0, 256) / 256f;
        if (rng < chance) return 2;
        else return 1;
    }
}
