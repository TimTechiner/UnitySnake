using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject fruitPrefab;

    public void Spawn(List<Vector2Int> freeCells)
    {
        if (freeCells == null || freeCells.Count == 0) return;

        var randomSpaceIndex = Random.Range(0, freeCells.Count);
        var randomSpace = freeCells[randomSpaceIndex];

        Instantiate(fruitPrefab, new Vector3(randomSpace.x, randomSpace.y), fruitPrefab.transform.rotation);
    }

    public void Remove(GameObject fruit)
    {
        Destroy(fruit);
    }
}
