using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySpawnData
{
    public Vector2Int Pos { get; set; }
    public int SpawnDelay { get; set; }
    public Snake Snake { get; set; }

    public BodySpawnData(Vector2Int pos, int spawnDelay, Snake snake)
    {
        Pos = pos;
        SpawnDelay = spawnDelay;
        Snake = snake;
    }
}
