using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private readonly int dirXHash = Animator.StringToHash("dirX");
    private readonly int dirYHash = Animator.StringToHash("dirY");
    private readonly int isMove = Animator.StringToHash("isMove");
    private readonly int shoot = Animator.StringToHash("shoot");
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayerWalkAnimation(float h, float v)
    {
        bool move = (Mathf.Abs(h) > 0.2f || Mathf.Abs(v) > 0.2f);
        animator.SetBool(isMove, move);
        animator.SetFloat(dirXHash, h);
        animator.SetFloat(dirYHash, v);
    }

    public void ShootAnimation()
    {
        animator.SetTrigger(shoot);
    }
}
