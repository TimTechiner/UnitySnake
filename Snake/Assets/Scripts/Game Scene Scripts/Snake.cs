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

    [SerializeField]
    private GameManager gameManager;

    private Vector2Int moveDirection;
    private int fieldSize = 20;

    private float movePeriod;
    private float currentMoveTime;

    private int length;
    private int turn;

    private bool isInputBlocked = false;

    public SnakeElement LastElement { get; private set; }

    public event EventHandler<OnTurnChangedEventArgs> OnTurnChanged;

    public class OnTurnChangedEventArgs : EventArgs
    {
        public int Turn { get; private set; }
        public OnTurnChangedEventArgs(int turn)
        {
            Turn = turn;
        }
    }

    private void Awake()
    {
        gridPos = new Vector2Int(fieldSize / 2,  fieldSize / 2);
        movePeriod = 1f / speed;
        currentMoveTime = 0f;
        moveDirection = new Vector2Int(0, 0);
        length = 1;
        LastElement = this;
    }

    private void Start()
    {
        gameManager.OnBodySpawned += GameManager_OnBodySpawned;
    }

    private void GameManager_OnBodySpawned(object sender, GameManager.OnBodySpawnedEventArgs e)
    {
        LastElement = e.Body;
    }

    private void Update()
    {
        HandleInput();

        HandleMovement();
    }

    private void HandleInput()
    {
        if (isInputBlocked) return;

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (moveDirection.y != -1)
            {
                moveDirection.x = 0;
                moveDirection.y = 1;
            }
            isInputBlocked = true;
        }
        else if (Input.GetKeyDown(KeyCode.S)) 
        {
            if (moveDirection.y != 1)
            {
                moveDirection.x = 0;
                moveDirection.y = -1;
            }
            isInputBlocked = true;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (moveDirection.x != -1)
            {
                moveDirection.x = 1;
                moveDirection.y = 0;
            }
            isInputBlocked = true;
        }
        else if (Input.GetKeyDown(KeyCode.A))
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

            OnTurnChanged?.Invoke(this, new OnTurnChangedEventArgs(++turn));

            isInputBlocked = false;
        }

        transform.position = new Vector3(gridPos.x, gridPos.y);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Fruit>() != null)
        {
            gameManager.SetScore(other.gameObject.GetComponent<Fruit>().Scores);

            Destroy(other.gameObject);
            gameManager.SpawnFruit(GetOccupiedSpace().ToList());

            gameManager.PutBodyInSpawnQueue(gridPos, turn + length, this);
            length++;
        }
        else if (other.gameObject.GetComponent<Body>())
        {
            gameManager.GameOver();
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
}
