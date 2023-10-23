using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Snake : SnakeElement
{
    [SerializeField]
    private float speed;

    private Vector2Int moveDirection;
    private int fieldSize = 20;

    private float movePeriod;
    private float currentMoveTime;

    private int length;
    private int turn;

    private bool isInputBlocked = false;

    public SnakeElement LastElement { get; private set; }

    public event EventHandler<FruitEatingEventArgs> FruitEating;

    public class FruitEatingEventArgs : EventArgs
    {
        public GameObject Fruit { get; private set; }
        public List<Vector2Int> OccupiedCells { get; private set; }
        public Vector2Int BodySpawnPosition { get; private set; }
        public int BodySpawnDelay { get; private set; }
        public FruitEatingEventArgs(GameObject fruit, List<Vector2Int> occupiedCells, Vector2Int bodySpawnPosition, int bodySpawnDelay)
        {
            Fruit = fruit;
            OccupiedCells = occupiedCells;
            BodySpawnPosition = bodySpawnPosition;
            BodySpawnDelay = bodySpawnDelay;
        }
    }

    public event EventHandler<EventArgs> BodyCollided;

    public event EventHandler<TurnChangedEventArgs> TurnChanged;
    public class TurnChangedEventArgs : EventArgs
    {
        public int Turn { get; private set; }
        public TurnChangedEventArgs(int turn)
        {
            Turn = turn;
        }
    }
    public event EventHandler<BodyClearedEventArgs> BodyCleared;
    public class BodyClearedEventArgs : EventArgs
    {
        public List<Body> Bodies { get; private set; }

        public BodyClearedEventArgs()
        {
            Bodies = new List<Body>();
        }
    }

    private void Awake()
    {
        movePeriod = 1f / speed;
        currentMoveTime = 0f;
    }

    public void Initialize()
    {
        gridPos = new Vector2Int(fieldSize / 2, fieldSize / 2);
        moveDirection = new Vector2Int(0, 0);
        length = 1;
        LastElement = this;

        ClearBodies();
        this.Child = null;
    }

    public void AddBody(Body body)
    {
        LastElement.Child = body;
        body.Parent = LastElement;
        LastElement = body;
    }

    private void Update()
    {
        HandleInput();

        HandleMovement();
    }

    private void HandleInput()
    {
        if (isInputBlocked) return;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (moveDirection.y != -1)
            {
                moveDirection.x = 0;
                moveDirection.y = 1;
            }
            isInputBlocked = true;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) 
        {
            if (moveDirection.y != 1)
            {
                moveDirection.x = 0;
                moveDirection.y = -1;
            }
            isInputBlocked = true;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (moveDirection.x != -1)
            {
                moveDirection.x = 1;
                moveDirection.y = 0;
            }
            isInputBlocked = true;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (moveDirection.x != 1)
            {
                moveDirection.x = -1;
                moveDirection.y = 0;
            }
            isInputBlocked = true;
        }
    }

    private void HandleMovement()
    {
        currentMoveTime += Time.deltaTime;

        if (currentMoveTime >= movePeriod)
        {
            SnakeElement_OnMoved(this, new OnMovedEventArgs(gridPos));

            gridPos += moveDirection;
            var x = (gridPos.x + fieldSize) % fieldSize; 
            var y = (gridPos.y + fieldSize) % fieldSize; 

            gridPos = new Vector2Int(x, y);
            currentMoveTime = 0;

            TurnChanged?.Invoke(this, new TurnChangedEventArgs(++turn));

            isInputBlocked = false;
        }

        transform.position = new Vector3(gridPos.x, gridPos.y);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Fruit>() != null)
        {
            FruitEatingEventArgs args = new FruitEatingEventArgs(
                fruit: other.gameObject, 
                occupiedCells: GetOccupiedSpace().ToList(), 
                bodySpawnPosition: gridPos, 
                bodySpawnDelay: turn + length);
            FruitEating?.Invoke(this, args);

            length++;
        }
        else if (other.gameObject.GetComponent<Body>())
        {
            BodyCollided?.Invoke(this, EventArgs.Empty);
        }
    }

    private IEnumerable<Vector2Int> GetOccupiedSpace()
    {
        SnakeElement snakeElement = this;

        while (snakeElement.Child != null)
        {
            yield return snakeElement.gridPos;
            snakeElement = snakeElement.Child;
        }

        yield return snakeElement.gridPos;
    }

    private void ClearBodies()
    {
        BodyClearedEventArgs args = new BodyClearedEventArgs();

        var body = this.Child;

        if (body == null) return;

        while (body.Child != null)
        {
            args.Bodies.Add(body as Body);
            body = body.Child;
        }

        args.Bodies.Add(body as Body);

        BodyCleared?.Invoke(this, args);
    }
}
