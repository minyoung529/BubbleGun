using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookIK : MonoBehaviour
{
    Animator anim;
    [SerializeField] private Transform target;
    private bool isIKStart = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        EventManager.StartListening("GameStart", SetIKStart);
    }

    private void Start()
    {
        //target = GameObject.Find("Target").transform;
    }

    private void SetIKStart()
    {
        isIKStart = true;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (isIKStart)
        {
            anim.SetIKPosition(AvatarIKGoal.RightHand, target.position);
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.5f);

            anim.SetIKPosition(AvatarIKGoal.LeftHand, target.position);
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.5f);
        }
    }

    private void OnDestroy()
    {
        EventManager.StopListening("GameStart", SetIKStart);
    }
}