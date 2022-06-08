using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    private ISkill currentSkill;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            currentSkill = new CircleShoot();
            currentSkill.OnEnterSkill();
        }

        currentSkill?.OnStaySkill();
    }
}
