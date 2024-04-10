using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

[Serializable]
public class Pokemon : BaseData
{
    [field: SerializeField] public int Level { get; set; } = 1;
    public int CurrentHealth { get; set; }
    [field: SerializeField] public Stats BaseStats { get; set; }
    [field: SerializeField] public Stats ScaledStats { get; set; }
    public ElementalType type1;
    public ElementalType type2;
    public List<Move> moves;
    public Texture Texture { get; set; }

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
        [field: SerializeField] public int Health { get; set;  }
        [field: SerializeField] public int Attack { get; set; }
        [field: SerializeField] public int Defense { get; set; }
        [field: SerializeField] public int SpAttack { get; set; }
        [field: SerializeField] public int SpDefense { get; set; }
        [field: SerializeField] public int Speed { get; set; }
    }

    public Pokemon(int id, string name, int baseHealth, int attack, int defense, int spAttack, int spDefense, int speed, ElementalType type1, ElementalType type2 = ElementalType.None) : base (id, name)
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

    public void AttackPokemon(Pokemon target, Move move)
    {
        if(Random.Range(0, 100) <= move.Accuracy)
        {
            TypeMultiplier typeMultiplierClass = new();
            int critMultiplier = CriticalMultiplier(ScaledStats.Speed / 2f / 256f);
            float typeMultiplier = typeMultiplierClass.DamageMultiplier(move.ElementalType, target.GetType()) * typeMultiplierClass.DamageMultiplier(move.ElementalType, target.GetType());
            float lostHP = ((2 * Level * critMultiplier / 5f + 2f) * move.Power * ScaledStats.Attack / ScaledStats.Defense / 50f + 2f) * typeMultiplier * (Random.Range(217, 256) / 255f);
            target.CurrentHealth -= (int)lostHP;
            Debug.Log($"{Name} attacks {target.Name}");
            if (lostHP >= 1)
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
        else
        {
            Debug.Log($"The attack missed!");
        }
    }

    private int CriticalMultiplier(float chance)
    {
        var rng = Random.Range(0, 256) / 256f;
        if (rng < chance) return 2;
        else return 1;
    }
}
