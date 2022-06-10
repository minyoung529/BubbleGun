using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juggling : ISkill
{
    public bool IsEnd { get; set; } = false;

    private const int SHOOT_COUNT = 18;
    private const float SHOOT_ANGLE = 360f / SHOOT_COUNT;

    private PlayerController character;
    private FireCtrl fireController;

    private float angle = 0f;
    private Vector3 rotation = Vector3.zero;
    private float curShootAngle = SHOOT_ANGLE;

    public void OnEnterSkill()
    {
        character = GameManager.Instance().PlayerController;
        fireController = character.GetComponent<FireCtrl>();

        rotation = character.transform.eulerAngles;
    }

    public void OnExitSkill()
    {
        IsEnd = true;
    }


    public void OnStaySkill()
    {
        if (angle > 360f)
        {
            OnExitSkill();
            return;
        }

        rotation = character.transform.eulerAngles * Time.deltaTime;

        Vector3 rot = character.transform.eulerAngles;
        rot.y = rotation.y + angle;
        character.transform.eulerAngles = rot;

        angle += Time.deltaTime * 360f;

        if (angle > curShootAngle)
        {
            curShootAngle += SHOOT_ANGLE;
            fireController.Shoot();
        }
    }
}
