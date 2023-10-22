using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject fruitPrefab;

    [SerializeField]
    private GameObject bodyPrefab;

    [SerializeField]
    private Snake snake;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private GameObject gameOverScreen;

    [SerializeField]
    private TextMeshProUGUI gameOverScreenScoreText;

    private int width = 20;
    private int height = 20;
    private int score;

    private Queue<BodySpawnData> bodySpawnQueue = new Queue<BodySpawnData>();

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

    private List<Vector2Int> field;

    public event EventHandler<OnBodySpawnedEventArgs> OnBodySpawned;

    public class OnBodySpawnedEventArgs : EventArgs
    {
        public Body Body { get; private set; }
        public OnBodySpawnedEventArgs(Body body)
        {
            Body = body;
        }
    }

    private void Awake()
    {
        field = new List<Vector2Int>();
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                field.Add(new Vector2Int(i, j));
            }
        }

        StartGame();
    }

    private void StartGame()
    {
        Time.timeScale = 1f;

        SetScore(0);

        SpawnFruit(new List<Vector2Int>() { new Vector2Int(width / 2, height / 2) });
    }

    private void Start()
    {
        snake.OnTurnChanged += Snake_OnTurnChanged;
    }

    private void Snake_OnTurnChanged(object sender, Snake.OnTurnChangedEventArgs e)
    {
        if (bodySpawnQueue.Count == 0 || e.Turn < bodySpawnQueue.Peek().SpawnDelay) return;

        var bodySpawnData = bodySpawnQueue.Dequeue();

        var pos = bodySpawnData.Pos;
        var snake = bodySpawnData.Snake;

        var spawnedBody = Instantiate(bodyPrefab, new Vector3(pos.x, pos.y), bodyPrefab.transform.rotation);

        var body = spawnedBody.GetComponent<Body>();

        body.Parent = snake.LastElement;
        snake.LastElement.Child = body;

        body.SetInitialPos(pos);

        OnBodySpawned?.Invoke(this, new OnBodySpawnedEventArgs(body));
    }

    public void SetScore(int score)
    {
        this.score += score;
        scoreText.text = $"Score: {this.score}";
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverScreenScoreText.text = score.ToString();

        scoreText.gameObject.SetActive(false);
        gameOverScreen.SetActive(true);
    }

    public void SpawnFruit(List<Vector2Int> lockedSpaces)
    {
        var freeSpaces = field.Except(lockedSpaces).ToList();

        var randomSpaceIndex = Random.RandomRange(0, freeSpaces.Count);
        var randomSpace = freeSpaces[randomSpaceIndex];

        Instantiate(fruitPrefab, new Vector3(randomSpace.x, randomSpace.y), transform.rotation);
    }

    public void PutBodyInSpawnQueue(Vector2Int pos, int delay, Snake snake)
    {
        BodySpawnData bodySpawnData = new BodySpawnData(pos, delay, snake);
        bodySpawnQueue.Enqueue(bodySpawnData);
    }
}
