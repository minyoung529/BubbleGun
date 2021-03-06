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

    [SerializeField] private Image hpBar;

    [Header("Canvas Group")]
    [SerializeField] private CanvasGroup gameOverCanvas;
    [SerializeField] private CanvasGroup shootingCanvas;
    [SerializeField] private CanvasGroup gameClearCanvas;

    [Header("UI")]
    [SerializeField] private Image dangerUI;
    [SerializeField] private Slider bossSlider;

    private void Awake()
    {
        EventManager.StartListening("GameOver", GameOver);
        EventManager.StartListening("Win", OnGameEnd);
    }

    public void UpdateHp(float curHp, float initHp)
    {
        hpBar.fillAmount = curHp / initHp;

        if ((curHp / initHp) > 0.2f)
            StartCoroutine(OnDamaged());
        else
            dangerUI.gameObject.SetActive(true);
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

        if (infoText.text.Contains("HP"))
        {
            infoText.text = $"{info}( {deadCnt}/{maxCnt} )";
        }
        else
        {
            infoText.text = $"{info}( {deadCnt}/{maxCnt} )";
        }
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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        UnShowCanvasGroup(shootingCanvas);

        Sequence seq = DOTween.Sequence().SetDelay(3f);
        seq.AppendCallback(() => ShowCanvasGroup(gameClearCanvas));
    }

    private void GameOver()
    {
        ShowCanvasGroup(gameOverCanvas);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private IEnumerator OnDamaged()
    {
        dangerUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        dangerUI.gameObject.SetActive(false);
    }

    public void UpdateBossHp(int maxhp, int curHp)
    {
        bossSlider.gameObject.SetActive(curHp > 0);
        bossSlider.value = (curHp / (float)maxhp);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("GameOver", GameOver);
        EventManager.StopListening("Win", OnGameEnd);
    }
}