using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    [SerializeField]
    private GameObject menuPopup;

    private void Start()
    {
        Button backButton = GetComponent<Button>();
        backButton.onClick.AddListener(() => menuPopup.SetActive(false));
    }
}
