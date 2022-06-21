using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookIK : MonoBehaviour
{
    Animator anim;
    [SerializeField] private Transform target;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        //target = GameObject.Find("Target").transform;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        anim.SetIKPosition(AvatarIKGoal.RightHand, target.position);
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.5f);

        anim.SetIKPosition(AvatarIKGoal.LeftHand, target.position);
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.5f);
    }
}
