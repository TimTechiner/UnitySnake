using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : SnakeElement
{
    
    public void SetInitialPos(Vector2Int pos)
    {
        gridPos = pos;
    }

    private void Update()
    {
        transform.position = new Vector3(gridPos.x, gridPos.y);
    }
}
