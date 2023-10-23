using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject fruitPrefab;

    private List<GameObject> spawnedFruits;

    private void Awake()
    {
        spawnedFruits = new List<GameObject>();
    }

    public void Spawn(List<Vector2Int> freeCells)
    {
        if (freeCells == null || freeCells.Count == 0) return;

        var randomSpaceIndex = Random.Range(0, freeCells.Count);
        var randomSpace = freeCells[randomSpaceIndex];

        var fruit = Instantiate(fruitPrefab, new Vector3(randomSpace.x, randomSpace.y), fruitPrefab.transform.rotation);

        spawnedFruits.Add(fruit);
    }

    public void Remove(GameObject fruit)
    {
        spawnedFruits.Remove(fruit);
        Destroy(fruit);
    }

    public void RemoveAll()
    {
        foreach (var fruit in spawnedFruits)
            Destroy(fruit);

        spawnedFruits.Clear();
    }
}
