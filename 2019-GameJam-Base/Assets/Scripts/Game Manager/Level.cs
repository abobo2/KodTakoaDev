using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Jam/Level", order = 1)]
public class Level : ScriptableObject
{
    public int level;
    public int time;
    public LevelTask[] tasks;
}
