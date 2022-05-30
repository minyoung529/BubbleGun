using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIMgr : MonoBehaviour
{
    // ��ư����
    public Button startButton;
    public Button optionButton;
    public Button shopButton;

    private UnityAction action;

    void Start()
    {
        // StartButton �̺�Ʈ����
        action = () => OnStartClick();
        startButton.onClick.AddListener(action);

        // ���� �޼��带 Ȱ���� �̺�Ʈ ����
        optionButton.onClick.AddListener(delegate { OnButtonClick(optionButton.name); });

        // ���ٽ� Ȱ���� �̺�Ʈ ���� ���
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
