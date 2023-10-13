using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Snake;

public class SnakeElement : MonoBehaviour
{
    public Vector2Int gridPos { get; protected set; }
    public SnakeElement Parent { get; set; }
    public SnakeElement Child { get; set; }

    public event EventHandler<OnMovedEventArgs> OnMoved;
    public class OnMovedEventArgs : EventArgs
    {
        public Vector2Int OldPos { get; private set; }
        public OnMovedEventArgs(Vector2Int oldPos)
        {
            OldPos = oldPos;
        }
    }

    private void Start()
    {
        if (Parent == null) return;

        SnakeElement snakeElement = Parent.GetComponent<SnakeElement>();

        if (snakeElement != null)
        {
            snakeElement.OnMoved += SnakeElement_OnMoved;
        }
    }

    protected void SnakeElement_OnMoved(object sender, OnMovedEventArgs e)
    {
        OnMoved?.Invoke(this, new OnMovedEventArgs(gridPos));
        gridPos = e.OldPos;
    }
}
