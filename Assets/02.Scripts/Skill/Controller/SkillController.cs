using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    private ISkill currentSkill;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            UseSkill(KeyCode.LeftShift, new Juggling());

        if (Input.GetKeyDown(KeyCode.E))
            UseSkill(KeyCode.E, new Firecracker());

        currentSkill?.OnStaySkill();
    }

    private void UseSkill(KeyCode keyCode, ISkill skill)
    {
        SkillPanel skillPanel = GameManager.Instance().skillPanels[keyCode];
        if (skillPanel.CanUseSkill())
        {
            currentSkill = skill;
            currentSkill.OnEnterSkill();

            skillPanel.UseSkill();
        }
    }
}
