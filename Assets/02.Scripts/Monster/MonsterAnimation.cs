using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimation : MonoBehaviour
{
    private readonly int hashTrace = Animator.StringToHash("IsTrace");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashStun = Animator.StringToHash("Stun");
    
    [SerializeField] private Animator anim;

    public void PlayHitAnimation()
    {
        anim.SetTrigger(hashHit);
    }

    public void PlayGangNamStyle()
    {
        anim.SetFloat(hashSpeed, Random.Range(0.8f, 1.3f));
        anim.SetTrigger(hashPlayerDie);
    }

    public void PlayAttackAnimation()
    {
        anim.SetBool(hashAttack, true);
    }

    public void PlayTraceAnimation()
    {
        anim.SetBool(hashTrace, true);
        anim.SetBool(hashAttack, false);
    }

    public void PlayDieAnimation()
    {
        anim.SetTrigger(hashDie);
    }

    public void PlayStunAnimation()
    {
        anim.SetTrigger(hashStun);
    }

    public void PlayIdleAnimation()
    {
        anim.SetBool(hashTrace, false);
    }
}
