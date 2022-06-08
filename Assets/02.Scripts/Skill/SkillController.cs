using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    private ISkill currentSkill;
    SkillPanel skillPanel;

    void Update()
    {
        skillPanel = GameManager.Instance().skillPanels[KeyCode.Q];

        if (Input.GetKeyDown(KeyCode.Q) && skillPanel.CanUseSkill())
        {
            currentSkill = new Juggling();
            currentSkill.OnEnterSkill();
            skillPanel.UseSkill();
        }

        currentSkill?.OnStaySkill();
    }
}
