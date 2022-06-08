using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShoot : ISkill
{
    PlayerController character;
    private float angle = 0f;
    private Vector3 rotation = Vector3.zero;

    public void OnEnterSkill()
    {
        character = GameManager.Instance().PlayerController;
        rotation = character.transform.eulerAngles;
        character.IsMove = true;
    }

    public void OnExitSkill()
    {
        character.IsMove = false;
    }

    public void OnStaySkill()
    {
        if (angle > 360f)
        {
            character.IsMove = false;
            return;
        }

        //character.GetComponent<FireCtrl>().Shoot();

        rotation = character.transform.eulerAngles * Time.deltaTime;

        Vector3 rot = character.transform.eulerAngles;
        rot.y = rotation.y + angle;
        character.transform.eulerAngles = rot;
        
        angle += Time.deltaTime * 90f;
    }
}
