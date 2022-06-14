using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup inGameUI;
    [SerializeField] private Text infoText;
    [SerializeField] private Text playerText;
    [SerializeField] private Text scoreText;

    [SerializeField] private Image hpBar;

    private void Start()
    {
        inGameUI.gameObject.SetActive(false);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void UpdateHp(float curHp, float initHp)
    {
        hpBar.fillAmount = curHp / initHp;
    }

    public void ShowInfoText(string info)
    {
        infoText.text = info;
        Sequence textSequence = DOTween.Sequence();
        textSequence.Append(infoText.transform.parent.DOScaleX(1f, 0.3f));
        textSequence.Insert(1.5f, infoText.transform.parent.DOScaleX(0f, 0.3f));
    }
}
