using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BodySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject bodyPrefab;

    private Queue<BodySpawnData> spawnQueue;

    private void Awake()
    {
        spawnQueue = new Queue<BodySpawnData>();
    }

    public void AddToQueue(Vector2Int pos, int delay, Snake snake)
    {
        BodySpawnData bodySpawnData = new BodySpawnData(pos, delay, snake);
        spawnQueue.Enqueue(bodySpawnData);
    } 

    public void Spawn()
    {

    }

    public void Remove(GameObject body)
    {
        Destroy(body);
    }
}
