using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveDatabase", menuName = "Database/Create Move Database")]
public class MoveDatabase : ScriptableObject
{
    public List<Move> moves;
}

