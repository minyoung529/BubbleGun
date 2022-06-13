using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Birthday : ISkill
{
    public bool IsEnd { get; set; }

    public void OnEnterSkill()
    {
        PlayerController.WeaponType = WeaponType.Hammer;
        GameObject.FindObjectOfType<WeaponCtrl>().Shoot();
    }

    public void OnExitSkill()
    {
        PlayerController.WeaponType = WeaponType.Gun;
    }

    public void OnStaySkill()
    {

    }
}
