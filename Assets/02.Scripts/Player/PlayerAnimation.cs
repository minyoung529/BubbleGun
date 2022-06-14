using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimation : MonoBehaviour
{
    private readonly int dirXHash = Animator.StringToHash("dirX");
    private readonly int dirYHash = Animator.StringToHash("dirY");
    private readonly int isMove = Animator.StringToHash("isMove");
    private readonly int shoot = Animator.StringToHash("shoot");
    private readonly int jump = Animator.StringToHash("jump");
    private readonly int weaponHash = Animator.StringToHash("Weapon");
    private readonly int standUpHash = Animator.StringToHash("standUp");

    private Animator animator;


    void Awake()
    {
        animator = GetComponent<Animator>();

        EventManager.StartListening("GameStart", SetEveryWeightActive);
    }

    private void SetEveryWeightActive()
    {
        animator.SetLayerWeight(1, 1f);
        animator.SetLayerWeight(2, 1f);
    }

    public void ChangeWeapon()
    {
        animator.SetInteger(weaponHash, (int)PlayerController.WeaponType);
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

    public void JumpAnimation()
    {
        animator.SetTrigger(jump);
    }

    public void StandUpAnimation()
    {
        animator ??= GetComponent<Animator>();

        animator.SetLayerWeight(1, 0f);
        animator.SetLayerWeight(2, 0f);
        animator.SetTrigger(standUpHash);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("GameStart", SetEveryWeightActive);
    }
}