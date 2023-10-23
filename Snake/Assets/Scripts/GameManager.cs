using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private FruitSpawner fruitSpawner;

    [SerializeField]
    private BodySpawner bodySpawner;

    [SerializeField]
    private Snake snake;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private GameObject gameOverScreen;

    [SerializeField]
    private TextMeshProUGUI gameOverScreenScoreText;

    [SerializeField]
    private TextMeshProUGUI gameOverScreenBestScoreText;

    [SerializeField]
    private PlayAgainButton playAgainButton;

    private int width = 20;
    private int height = 20;
    private int score;
    private int bestScore;

    private List<Vector2Int> field;

    private void Awake()
    {
        LoadSavedData();

        InitializeField();

        StartGame();
    }

    private void LoadSavedData()
    {
        var savedData = FileManager.Load();
        bestScore = savedData.bestScore;
    }

    private void SaveData(int newBestScore)
    {
        SaveData saveData = new SaveData() { bestScore = newBestScore };
        FileManager.Save(saveData);
    }

    private void InitializeField()
    {
        field = new List<Vector2Int>();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                field.Add(new Vector2Int(i, j));
            }
        }
    }

    private void StartGame()
    {
        gameOverScreen.SetActive(false);

        scoreText.gameObject.SetActive(true);

        ClearField();

        Time.timeScale = 1f;

        SetScore(0);

        SpawnFruit(null, Enumerable.Empty<Vector2Int>().ToList());

        snake.Initialize();

        bodySpawner.Initialize(snake);
    }

    private void ClearField()
    {
        fruitSpawner.RemoveAll();
    }

    private void Start()
    {
        snake.TurnChanged += Snake_OnTurnChanged;
        snake.FruitEating += OnFruitEating;
        snake.BodyCollided += OnBodyCollided;
        snake.BodyCleared += OnBodyCleared;

        playAgainButton.PlayAgainButtonClicked += OnPlayAgainButtonClicked;
    }

    private void OnBodyCleared(object sender, Snake.BodyClearedEventArgs e)
    {
        foreach (var body in e.Bodies)
        {
            bodySpawner.Remove(body.gameObject);
        }
    }

    private void OnPlayAgainButtonClicked(object sender, EventArgs e)
    {
        StartGame();
    }

    private void OnBodyCollided(object sender, EventArgs e)
    {
        GameOver();
    }

    private void OnFruitEating(object sender, Snake.FruitEatingEventArgs e)
    {
        SpawnFruit(e.Fruit, e.OccupiedCells);
        PutBodyInSpawnQueue(e.BodySpawnPosition, e.BodySpawnDelay);
        IncrementScoreBy(1);
    }

    private void Snake_OnTurnChanged(object sender, Snake.TurnChangedEventArgs e)
    {
        TrySpawnBody(e.Turn);
    }

    private void IncrementScoreBy(int score)
    {
        SetScore(this.score + score);
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = $"Score: {this.score}";
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
        gameOverScreenScoreText.text = score.ToString();

        if (score > bestScore)
        {
            SaveData(score);
            bestScore = score;
        }

        gameOverScreenBestScoreText.text = bestScore.ToString();

        scoreText.gameObject.SetActive(false);
        gameOverScreen.SetActive(true);
    }

    private void SpawnFruit(GameObject fruit, List<Vector2Int> occupiedCells)
    {
        fruitSpawner.Remove(fruit);
        fruitSpawner.Spawn(field.Except(occupiedCells).ToList());
    }

    private void TrySpawnBody(int turn)
    {
        bodySpawner.TrySpawn(turn);
    }

    private void PutBodyInSpawnQueue(Vector2Int pos, int delay)
    {
        bodySpawner.AddToQueue(pos, delay);
    }
}
