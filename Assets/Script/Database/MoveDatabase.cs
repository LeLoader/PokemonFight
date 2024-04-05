using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveDatabase", menuName = "Database/Create Move Database")]
public class MoveDatabase : ScriptableObject
{
    [SerializeField] public List<Move> moves;
}

