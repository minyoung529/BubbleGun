using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text infoText;
    [SerializeField] private Text playerText;
    [SerializeField] private Text scoreText;

    [SerializeField] private Image hpBar;

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

    public void ShowCanvasGroup(CanvasGroup group)
    {
        group.alpha = 0f;
        group.DOFade(1f, 1f);
    }

    public void UnShowCanvasGroup(CanvasGroup group)
    {
        group.DOFade(0f, 1f);
    }
}
