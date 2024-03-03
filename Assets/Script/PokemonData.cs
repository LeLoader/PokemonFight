using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PokemonData : MonoBehaviour
{
    [SerializeField] new string name;
    [SerializeField] int maxHealth;
    int health;
    [SerializeField] int attack;
    [SerializeField] int defense;
    int stats;
    [SerializeField] float weight;
    [SerializeField] enum PokemonType {Fire, Water, Grass, Poison}
    [SerializeField] PokemonType[] resistances;
    [SerializeField] PokemonType[] weaknesses;

    void Awake(){
        //InitCurrentLife();
        //InitStatsPoints();
        //DisplayAll();
    }

    void Update(){
        //CheckIfPokemonAlive();
    }

    void InitCurrentLife(){
        health = maxHealth;
    }

    void InitStatsPoints(){
        stats = health + attack + defense;
    }

    int GetAttackDamage(){
        return defense;
    }

    void TakeDamage(int damage, PokemonType offenseType){
        float multiplicator = 1f;
        foreach (PokemonType defenseType in weaknesses){
            if(defenseType == offenseType) multiplicator *= 2;
        }
        foreach (PokemonType defenseType in resistances){
            if(defenseType == offenseType) multiplicator *= 0.5f;
        }
        int finalDamage = (int)(damage * multiplicator);
        if(finalDamage > 0) health -= finalDamage;
        SetHealth((int)(damage * multiplicator), true);
    }

    /// <param name="hp">HP to modify</param>
    /// <param name="exterior">True for external source of damage and false for internal source of healing</param>
    void SetHealth(int hp, bool external){
        if(external && hp > 0) health -= hp;
        if(!external && hp > 0){
            health = Mathf.Max(health + hp, maxHealth);
        }
    }

    void CheckIfPokemonAlive(){
        if (health < 0) print($"Pokemon is alive and has {health} hp.");
        else print($"Pokemon {name} is dead. RIP");
    }

    void DisplayAll(){
        print($"Pokemon name: {name}");
        print($"Health: {health}");
        print($"Attack: {attack}");
        print($"Defense: {defense}");
        print($"Stats points: {stats}");
        print($"Weight: {weight}");
        print($"Resistance(s) types:");
        foreach (PokemonType resistance in resistances){
            print(resistance);
        }
        print($"Weakness(es) types:");
        foreach (PokemonType weakness in weaknesses){
            print(weakness);
        }
    }

    /// <summary>
    ///
    /// </summary>
    void isDead(){
        int hp = 0;
        if (hp <= 0) print("Player dead");
    }
}
