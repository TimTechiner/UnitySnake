using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectPlayModeButton : MonoBehaviour
{
    [SerializeField]
    private PlayMode playMode;

    private const string GAME_SCENE_TITLE = "Game";

    private void Start()
    {
        Button selectPlayModeButton = GetComponent<Button>();
        selectPlayModeButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        SceneManager.LoadScene(GAME_SCENE_TITLE);
    }
}
