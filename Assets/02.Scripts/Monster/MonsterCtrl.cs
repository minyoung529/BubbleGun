using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MonsterCtrl : MonoBehaviour
{
    // 몬스터의 상태 정보
    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE,
        PLAYERDIE,
    }

    // 몬스터의 현재 상태
    protected State state = State.IDLE;
    // 추적 사정 거리
    [SerializeField]
    protected float traceDist = 10.0f;
    // 공격 사정 거리

    [SerializeField]
    protected float attackDist = 2.0f;
    // 몬스터의 사망 여부
    protected bool isDie = false;

    protected Transform targetTransform;
    protected NavMeshAgent agent;
    protected Animator anim;

    // Animator 해쉬 값 추출
    protected readonly int hashTrace = Animator.StringToHash("IsTrace");
    protected readonly int hashAttack = Animator.StringToHash("IsAttack");
    protected readonly int hashHit = Animator.StringToHash("Hit");
    protected readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    protected readonly int hashSpeed = Animator.StringToHash("Speed");
    protected readonly int hashDie = Animator.StringToHash("Die");
    protected readonly int hashStun = Animator.StringToHash("Stun");

    // 혈흔 효과 프리팹
    protected GameObject bloodEffect;

    // 몬스터 생명 초기값
    [SerializeField]
    protected int initHp = 100;
    protected int currHp;

    [SerializeField] protected GameObject gumItem;

    public bool IsMove { get; set; } = true;

    protected virtual void Awake()
    {
        targetTransform = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();

        agent = GetComponent<NavMeshAgent>();

        // NavMeshAgent 자동 회전 기능 비활성화
        agent.updateRotation = false;

        anim = GetComponentInChildren<Animator>();

        bloodEffect = Resources.Load<GameObject>("BloodSprayEffect");
    }

    protected virtual void OnEnable()
    {
        state = State.IDLE;

        currHp = initHp;
        isDie = false;

        // 몬스터 콜라이더 활성화
        GetComponent<CapsuleCollider>().enabled = true;
        // 몬스터 펀치 콜라이더 활성화
        SphereCollider[] spheres = GetComponentsInChildren<SphereCollider>();
        foreach (SphereCollider sphere in spheres)
        {
            sphere.enabled = true;
        }

        // 몬스터의 상태를 체크하는 코루틴 함수
        StartCoroutine(CheckMonsterState());

        // 상태에 따라 몬스터의 행동을 수행하는 코루틴 함수
        StartCoroutine(MonsterAction());
    }

    protected virtual void Update()
    {
        // 목적지까지 남은 거리로 회전 여부 판단
        if (!IsMove) return;

        if (agent.remainingDistance >= 2.0f)
        {
            // 에이전의 이동 회전
            Vector3 direction = agent.desiredVelocity;

            if (direction.sqrMagnitude < 0.01f)
                return;

            Quaternion rot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 10.0f);
        }

        Vector3 animPos = anim.transform.position;
        animPos.y = 0f;

        anim.transform.position = animPos;
    }

    protected IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.3f);

            // 몬스터 상태가 DIE 코루틴 종류
            if (state == State.DIE)
                yield break;

            if (state == State.PLAYERDIE)
                yield break;

            // 몬스터와 주인공 캐릭터 사이의 거리 측정
            float distance = Vector3.Distance(transform.position, targetTransform.position);

            if (distance <= attackDist)
            {
                state = State.ATTACK;
            }
            else if (distance <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }
        }
    }

    protected IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            if (IsMove)
            {
                switch (state)
                {
                    case State.IDLE:
                        OnIdle();
                        break;

                    case State.TRACE:
                        OnTrace();
                        break;

                    case State.ATTACK:
                        OnAttack();
                        break;

                    case State.DIE:
                        StartCoroutine(OnDie());
                        break;

                    case State.PLAYERDIE:
                        OnPlayerDie();
                        break;
                }
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            anim.SetTrigger(hashHit);

            Vector3 pos = collision.GetContact(0).point;
            Quaternion rot = Quaternion.LookRotation(-collision.GetContact(0).normal);

            ShowBloodEffect(pos, rot);

            currHp -= 10;
            if (currHp <= 0)
            {
                state = State.DIE;
            }

            Destroy(collision.gameObject);
        }
    }

    void ShowBloodEffect(Vector3 pos, Quaternion rot)
    {
        GameObject blood = Instantiate(bloodEffect, pos, rot, transform);
        Destroy(blood, 1.0f);
    }

    private void PlayerDie()
    {
        state = State.PLAYERDIE;
    }

    #region State
    protected virtual void OnPlayerDie()
    {
        StopAllCoroutines();
        agent.isStopped = true;
        anim.SetFloat(hashSpeed, Random.Range(0.8f, 1.3f));
        anim.SetTrigger(hashPlayerDie);
    }

    protected virtual void OnAttack()
    {
        anim.SetBool(hashAttack, true);
    }

    protected virtual void OnIdle()
    {
        agent.isStopped = false;
        agent.isStopped = true;
        anim.SetBool(hashTrace, false);
    }

    protected virtual void OnTrace()
    {
        agent.SetDestination(targetTransform.position);
        agent.isStopped = false;
        anim.SetBool(hashTrace, true);
        anim.SetBool(hashAttack, false);
    }

    protected virtual IEnumerator OnDie()
    {
        isDie = true;
        agent.isStopped = true;
        anim.SetTrigger(hashDie);
        GetComponent<CapsuleCollider>().enabled = false;
        SphereCollider[] spheres = GetComponentsInChildren<SphereCollider>();
        foreach (SphereCollider sphere in spheres)
        {
            sphere.enabled = false;
        }

        Instantiate(gumItem, transform.position + Vector3.up * 2f, Quaternion.identity, null);

        yield return new WaitForSeconds(3.0f);

        gameObject.SetActive(false);
    }

    public virtual void Stun()
    {
        IsMove = false;
        agent.isStopped = true;
        anim.SetTrigger(hashStun);
    }
    #endregion

    void OnDrawGizmos()
    {
        if (state == State.TRACE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, traceDist);
        }
        if (state == State.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDist);
        }
    }
}