using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [Header("Game")]
    [SerializeField] private Text infoText;
    [SerializeField] private Text playerText;
    [SerializeField] private Text scoreText;

    [SerializeField] private Image hpBar;

    [Header("Canvas Group")]
    [SerializeField] private CanvasGroup gameOverCanvas;
    [SerializeField] private CanvasGroup shootingCanvas;
    [SerializeField] private CanvasGroup gameClearCanvas;

    [Header("UI")]
    [SerializeField] private Image dangerUI;

    private void Awake()
    {
        EventManager.StartListening("GameOver", () => ShowCanvasGroup(gameOverCanvas));
        EventManager.StartListening("Win", () => UnShowCanvasGroup(shootingCanvas));
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void UpdateHp(float curHp, float initHp)
    {
        hpBar.fillAmount = curHp / initHp;

        ShowDamageEffect((curHp / initHp) <= 0.2f);
    }

    public void ShowInfoText(string info)
    {
        StartCoroutine(InfoTextCoroutine(info));
    }

    private IEnumerator InfoTextCoroutine(string info)
    {
        infoText.transform.parent.DOScaleX(1f, 0.3f);
        yield return new WaitForSeconds(0.3f);

        foreach (string s in info.Split(','))
        {
            infoText.text = s.TrimEnd('&');
            yield return new WaitForSeconds(3f);
        }

        if (info.Length != 0 && info[info.Length - 1] != '&')
        {
            infoText.transform.parent.DOScaleX(0f, 0.3f);
        }
    }

    public void UpdateInfo(int maxCnt, int deadCnt)
    {
        string info = infoText.text.Split('(')[0];

        infoText.text = $"{info}( {deadCnt}/{maxCnt} )";
    }

    public void ShowCanvasGroup(CanvasGroup group)
    {
        group.gameObject.SetActive(true);
        group.alpha = 0f;
        group.DOFade(1f, 2f);
    }

    public void UnShowCanvasGroup(CanvasGroup group)
    {
        group.DOFade(0f, 1f).OnComplete(() => group.gameObject.SetActive(false));
    }

    public void OnGameEnd()
    {
        ShowCanvasGroup(gameClearCanvas);
    }

    public void ShowDamageEffect(bool isShow)
    {
        if (dangerUI.gameObject.activeSelf == isShow) return;

        dangerUI.gameObject.SetActive(isShow);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("GameOver", () => ShowCanvasGroup(gameOverCanvas));
        EventManager.StopListening("Win", () => UnShowCanvasGroup(shootingCanvas));
    }
}