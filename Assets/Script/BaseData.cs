using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseData
{
    public BaseData(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; set; }
    public string Name { get; set; }
}
