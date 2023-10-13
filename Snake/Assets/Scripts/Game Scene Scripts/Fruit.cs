using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    [SerializeField]
    private int scores;

    public int Scores => scores;

    private Vector2Int GridPos;
}
