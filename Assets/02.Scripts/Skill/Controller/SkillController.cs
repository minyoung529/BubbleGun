using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    private ISkill currentSkill;

    void Update()
    {
        if (currentSkill != null && currentSkill.IsEnd)
        {
            currentSkill = null;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
            UseSkill(KeyCode.LeftShift, new Juggling());

        if (Input.GetKeyDown(KeyCode.E))
            UseSkill(KeyCode.E, new Firecracker());

        if (Input.GetKeyDown(KeyCode.Q))
            UseSkill(KeyCode.Q, new ShootingStar(), false);

        if (Input.GetKeyDown(KeyCode.R))
            UseSkill(KeyCode.R, new Birthday(), false);

        currentSkill?.OnStaySkill();
    }

    private void UseSkill(KeyCode keyCode, ISkill skill, bool isAutoUse = true)
    {
        SkillPanel skillPanel = GameManager.Instance.skillPanels[keyCode];

        if (skillPanel.CanUseSkill())
        {
            currentSkill = skill;
            currentSkill.OnEnterSkill();

            if (isAutoUse)
            {
                skillPanel.UseSkill();
            }
        }
    }
}
