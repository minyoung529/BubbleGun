using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanel : MonoBehaviour
{
    // 스킬 활성 버튼
    [SerializeField]  private KeyCode keyCode;

    [SerializeField]  private float coolingTime;

    [SerializeField] private Image filledImage;

    private void Awake()
    {
        GameManager.Instance().skillPanels.Add(keyCode, this);
    }

    void Update()
    {
        if(filledImage.fillAmount > 0f)
        {
            filledImage.fillAmount -= Time.deltaTime / coolingTime;
        }
    }

    public void UseSkill()
    {
        filledImage.fillAmount = 1f;
    }

    public bool CanUseSkill()
    {
        return (filledImage.fillAmount <= 0.01f);
    }
}
