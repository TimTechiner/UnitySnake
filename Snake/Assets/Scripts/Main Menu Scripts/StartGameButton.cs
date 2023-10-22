using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    [SerializeField]
    private GameObject menuPopup;

    private void Start()
    {
        Button startGameButton = GetComponent<Button>();
        startGameButton.onClick.AddListener(() => menuPopup.SetActive(true));
    }
}
