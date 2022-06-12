using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // 버튼연결
    public Button startButton;
    public Button optionButton;
    public Button shopButton;

    private UnityAction action;

    void Start()
    {
        // StartButton 이벤트연결
        action = () => OnStartClick();
        startButton.onClick.AddListener(action);

        // 무명 메서드를 활용한 이벤트 연결
        optionButton.onClick.AddListener(delegate { OnButtonClick(optionButton.name); });

        // 람다식 활용한 이벤트 연결 방식
        shopButton.onClick.AddListener(() => OnButtonClick(shopButton.name));
    }

    void OnStartClick()
    {
        SceneManager.LoadScene("SampleScene");
    }

    void OnButtonClick(string str)
    {
        Debug.Log($"Click Button : { str}");
    }
}
