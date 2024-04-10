using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Move : BaseData
{
    public string[] learnedByPokemons;
    [field: SerializeField] public int Accuracy { get; set; }
    [field: SerializeField] public int Power { get; set; }
    [field: SerializeField] public MoveType MoveType { get; set; }
    [field: SerializeField] public ElementalType ElementalType { get; set; }

    public Move(int id, string name, int? accuracy, int? power, MoveType moveType, ElementalType elementalType) : base(id, name)
    {
        Id = id;
        Name = name;
        if (accuracy == null) Accuracy = 100;
        else Accuracy = (int)accuracy;
        if (power == null) Power = 0;
        else Power = (int)power;
        MoveType = moveType;
        ElementalType = elementalType;
    }
}
