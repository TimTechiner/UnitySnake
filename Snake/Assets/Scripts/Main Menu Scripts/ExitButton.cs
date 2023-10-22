using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    private void Start()
    {
        Button exitButton = GetComponent<Button>();
        exitButton.onClick.AddListener(() => Application.Quit());
    }
}
