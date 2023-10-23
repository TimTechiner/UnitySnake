using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject bodyPrefab;

    private Queue<BodySpawnData> spawnQueue;

    private Snake snake;

    public void Initialize(Snake snake)
    {
        this.snake = snake;
        spawnQueue = new Queue<BodySpawnData>();
    }

    public void AddToQueue(Vector2Int pos, int delay)
    {
        BodySpawnData bodySpawnData = new BodySpawnData(pos, delay);
        spawnQueue.Enqueue(bodySpawnData);
    } 

    public void TrySpawn(int turn)
    {
        if (spawnQueue.Count == 0 || turn < spawnQueue.Peek().SpawnDelay) return;

        var bodySpawnData = spawnQueue.Dequeue();

        var pos = bodySpawnData.Pos;

        var spawnedBody = Instantiate(bodyPrefab, new Vector3(pos.x, pos.y), bodyPrefab.transform.rotation);

        var body = spawnedBody.GetComponent<Body>();

        snake.AddBody(body);

        body.SetInitialPos(pos);
    }

    public void Remove(GameObject body)
    {
        Destroy(body);
    }
}
