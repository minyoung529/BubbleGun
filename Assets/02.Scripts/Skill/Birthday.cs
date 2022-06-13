using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Birthday : ISkill
{
    public bool IsEnd { get; set; }

    public void OnEnterSkill()
    {
        GameManager.Instance.PlayerController.SetWeaponType(WeaponType.Hammer);
        GameObject.FindObjectOfType<WeaponCtrl>().Shoot();
    }

    public void OnExitSkill()
    {
        GameManager.Instance.PlayerController.SetWeaponType(WeaponType.Gun);
    }

    public void OnStaySkill()
    {

    }
}
