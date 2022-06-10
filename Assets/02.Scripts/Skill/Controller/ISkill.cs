using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    public bool IsEnd { get; set; }
    public void OnEnterSkill();
    public void OnStaySkill();
    public void OnExitSkill();
}
