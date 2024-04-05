using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Move : BaseData
{
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

    int Accuracy { get; set; }
    int Power { get; set; }
    MoveType MoveType { get; set; }
    ElementalType ElementalType { get; set; }
}
