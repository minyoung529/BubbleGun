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

    [SerializeField] private Sound[] sounds;

    private void Awake()
    {
        GameManager.Instance.SkillPanels.Add(keyCode, this);
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

        foreach(Sound sound in sounds)  
        {
            SoundManager.Instance.PlayOneShot(sound.chanel, sound.clip);
        }
    }

    public bool CanUseSkill()
    {
        return (filledImage.fillAmount <= 0.01f);
    }
}
