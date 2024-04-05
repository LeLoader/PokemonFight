using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TypeMultiplier
{
    Type mult2 = new Type
    {

    };

    // STRUCT OR MATRIX
    //EFFECTIVEATTACK
    readonly Dictionary<ElementalType, ElementalType[]> effectiveAttack = new()
    {
        {ElementalType.Normal, new ElementalType[]{}},
        {ElementalType.Fire, new ElementalType[]{ ElementalType.Grass, ElementalType.Ice, ElementalType.Bug, ElementalType.Steel}},
        {ElementalType.Water, new ElementalType[]{ ElementalType.Fire, ElementalType.Ground, ElementalType.Rock}},
        {ElementalType.Grass, new ElementalType[]{ ElementalType.Water, ElementalType.Ground, ElementalType.Rock}},
        {ElementalType.Electric, new ElementalType[]{ ElementalType.Water, ElementalType.Flying}},
        {ElementalType.Ice, new ElementalType[]{ ElementalType.Electric, ElementalType.Ground, ElementalType.Flying, ElementalType.Dragon}},
        {ElementalType.Fighting, new ElementalType[]{ ElementalType.Normal, ElementalType.Ice, ElementalType.Rock, ElementalType.Dark, ElementalType.Steel}},
        {ElementalType.Poison, new ElementalType[]{ ElementalType.Grass}},
        {ElementalType.Ground, new ElementalType[]{ ElementalType.Fire, ElementalType.Electric, ElementalType.Poison, ElementalType.Rock, ElementalType.Steel}},
        {ElementalType.Flying, new ElementalType[]{ ElementalType.Grass, ElementalType.Fighting, ElementalType.Bug}},
        {ElementalType.Psychic, new ElementalType[]{ ElementalType.Fighting, ElementalType.Poison}},
        {ElementalType.Bug, new ElementalType[]{ ElementalType.Grass, ElementalType.Psychic, ElementalType.Dark}},
        {ElementalType.Rock, new ElementalType[]{ ElementalType.Fire, ElementalType.Ice, ElementalType.Flying, ElementalType.Bug}},
        {ElementalType.Ghost, new ElementalType[]{ ElementalType.Psychic, ElementalType.Ghost}},
        {ElementalType.Dragon, new ElementalType[]{ ElementalType.Dragon}},
        {ElementalType.Dark, new ElementalType[]{ ElementalType.Psychic, ElementalType.Ghost}},
        {ElementalType.Steel, new ElementalType[]{ ElementalType.Ice, ElementalType.Rock}},
        {ElementalType.Fairy, new ElementalType[]{ ElementalType.Fighting, ElementalType.Dark, ElementalType.Dragon}},
    };

    //WEAKATTACK
    readonly Dictionary<ElementalType, ElementalType[]> weakAttack = new()
    {
        {ElementalType.Normal, new ElementalType[]{ ElementalType.Rock, ElementalType.Steel}}, //NORMAL > DARK = 0
        {ElementalType.Fire, new ElementalType[]{ ElementalType.Fire, ElementalType.Water, ElementalType.Rock, ElementalType.Dragon}},
        {ElementalType.Water, new ElementalType[]{ ElementalType.Water, ElementalType.Grass, ElementalType.Dragon}},
        {ElementalType.Grass, new ElementalType[]{ ElementalType.Fire, ElementalType.Grass, ElementalType.Poison, ElementalType.Flying, ElementalType.Bug, ElementalType.Dragon, ElementalType.Steel}},
        {ElementalType.Electric, new ElementalType[]{ ElementalType.Grass, ElementalType.Electric, ElementalType.Dragon}}, //ELECTRIK > GROUND = 0
        {ElementalType.Ice, new ElementalType[]{ ElementalType.Fire, ElementalType.Water, ElementalType.Ice, ElementalType.Steel}},
        {ElementalType.Fighting, new ElementalType[]{ ElementalType.Poison, ElementalType.Ground, ElementalType.Psychic, ElementalType.Poison}}, //FIGHTING > GHOST = 0  
        {ElementalType.Poison, new ElementalType[]{ ElementalType.Poison, ElementalType.Ground, ElementalType.Rock, ElementalType.Ghost}}, //POISON > STEEL = 0
        {ElementalType.Ground, new ElementalType[]{ ElementalType.Grass, ElementalType.Bug}}, //GROUND > FLYING = 0
        {ElementalType.Flying, new ElementalType[]{ ElementalType.Electric, ElementalType.Rock, ElementalType.Steel}},
        {ElementalType.Psychic, new ElementalType[]{ ElementalType.Psychic, ElementalType.Steel}}, //PSYCHIC > DARK = 0
        {ElementalType.Bug, new ElementalType[]{ ElementalType.Fire, ElementalType.Fighting, ElementalType.Poison, ElementalType.Flying, ElementalType.Ghost, ElementalType.Steel}},
        {ElementalType.Rock, new ElementalType[]{ ElementalType.Fighting, ElementalType.Flying, ElementalType.Steel}},
        {ElementalType.Ghost, new ElementalType[]{ ElementalType.Dark, ElementalType.Steel}}, //GHOST > NORMAL = 0
        {ElementalType.Dragon, new ElementalType[]{ ElementalType.Steel}},
        {ElementalType.Dark, new ElementalType[]{ ElementalType.Fighting, ElementalType.Dark, ElementalType.Steel}},
        {ElementalType.Steel, new ElementalType[]{ ElementalType.Fire, ElementalType.Water, ElementalType.Steel}},
        {ElementalType.Fairy, new ElementalType[]{ ElementalType.Fire, ElementalType.Steel, ElementalType.Poison}},
    };

    struct Type //DO THAT FOR EACH TYPE; IMPLEMENTATION?
    {
        ElementalType attackingType;
        ElementalType[] effectiveType;
        ElementalType[] weakType;
        ElementalType nullifyingType;

        public Type(ElementalType attackingType, ElementalType[] effectiveType, ElementalType[] weakType, ElementalType nullifyingType)
        {
            this.attackingType = attackingType;
            this.effectiveType = effectiveType;
            this.weakType = weakType;
            this.nullifyingType = nullifyingType;
        }
    }


    public float DamageMultiplier(ElementalType sourceType, ElementalType[] targetType)
    {
        float damageMultiplier = 1f;
        if (sourceType != ElementalType.None)
        {
            foreach (ElementalType type in targetType)
            {
                if (effectiveAttack[sourceType].Contains(type))
                {
                    damageMultiplier *= 2f;
                }
            }
            foreach (ElementalType type in targetType)
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
