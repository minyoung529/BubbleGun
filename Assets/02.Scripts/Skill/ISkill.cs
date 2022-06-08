using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    public void OnEnterSkill();
    public void OnStaySkill();
    public void OnExitSkill();
}
