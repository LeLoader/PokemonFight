using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BaseData
{
    public BaseData(int id, string name)
    {
        Id = id;
        Name = name;
    }

    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public int Id { get; set; }
}
