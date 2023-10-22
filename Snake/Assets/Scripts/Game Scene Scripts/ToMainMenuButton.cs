using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToMainMenuButton : MonoBehaviour
{
    private const string MENU_SCENE_TITLE = "StartMenu";

    private void Start()
    {
        Button toMainMenuButton = GetComponent<Button>();
        toMainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene(MENU_SCENE_TITLE);
    }
}
