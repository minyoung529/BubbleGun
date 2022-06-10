using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanel : MonoBehaviour
{
    // 스킬 활성 버튼
    [SerializeField]  private KeyCode keyCode;

    // 스킬 쿨 타임
    [SerializeField]  private float coolingTime;

    // 스킬 쿨 타임 이미지
    [SerializeField] private Image filledImage;

    private void Awake()
    {
        GameManager.Instance.skillPanels.Add(keyCode, this);
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
