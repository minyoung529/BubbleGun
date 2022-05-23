using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{
    public Button startButton;
    public Button optionButton;
    public Button shopButton;

    private UnityAction action;

    private void Start()
    {
        action = () => OnStartClick();
        startButton.onClick.AddListener(action);

        optionButton.onClick.AddListener(delegate { OnButtonClick(optionButton.name); });

        shopButton.onClick.AddListener(() => OnButtonClick(optionButton.name));
    }

    private void OnButtonClick(string name)
    {
        Debug.Log($"Click Button: {name}");
    }

    private void OnStartClick()
    {
        SceneManager.LoadScene("Game");
    }
}
