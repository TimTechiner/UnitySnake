using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayAgainButton : MonoBehaviour
{
    private Button playAgainButton;

    public event EventHandler<EventArgs> PlayAgainButtonClicked;

    private void Start()
    {
        playAgainButton = GetComponent<Button>();
        playAgainButton.onClick.AddListener(() =>
        {
            PlayAgainButtonClicked?.Invoke(this, EventArgs.Empty);
        });
    }

    private void OnDestroy()
    {
        playAgainButton.onClick.RemoveAllListeners();
    }
}
