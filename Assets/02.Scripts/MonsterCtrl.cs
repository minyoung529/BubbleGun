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
    public State state = State.IDLE;
    // 추적 사정 거리
    public float traceDist = 10.0f;
    // 공격 사정 거리
    public float attackDist = 2.0f;
    // 몬스터의 사망 여부
    public bool isDie = false;

    private Transform monsterTransform;
    private Transform targetTransform;
    private NavMeshAgent agent;
    private Animator anim;

    // Animator 해쉬 값 추출
    private readonly int hashTrace = Animator.StringToHash("IsTrace");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");

    // 혈흔 효과 프리팹
    private GameObject bloodEffect;

    // 몬스터 생명 초기값
    private int initHp = 100;
    private int currHp;

    void Awake()
    {
        monsterTransform = GetComponent<Transform>();

        targetTransform = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();

        agent = GetComponent<NavMeshAgent>();

        // NavMeshAgent 자동 회전 기능 비활성화
        agent.updateRotation = false;

        anim = GetComponentInChildren<Animator>();

        bloodEffect = Resources.Load<GameObject>("BloodSprayEffect");
    }

    void OnEnable()
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

    void Update()
    {
        // 목적지까지 남은 거리로 회전 여부 판단
        if (agent.remainingDistance >= 2.0f)
        {
            // 에이전의 이동 회전
            Vector3 direction = agent.desiredVelocity;

            if (direction.sqrMagnitude < 0.01f)
                return;

            Quaternion rot = Quaternion.LookRotation(direction);


            monsterTransform.rotation = Quaternion.Slerp(monsterTransform.rotation, rot, Time.deltaTime * 10.0f);
        }
    }

    IEnumerator CheckMonsterState()
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
            float distance = Vector3.Distance(monsterTransform.position, targetTransform.position);

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

    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (state)
            {
                case State.IDLE:
                    agent.isStopped = true;
                    anim.SetBool(hashTrace, false);
                    break;
                case State.TRACE:
                    agent.SetDestination(targetTransform.position);
                    agent.isStopped = false;
                    anim.SetBool(hashTrace, true);
                    anim.SetBool(hashAttack, false);
                    break;
                case State.ATTACK:
                    anim.SetBool(hashAttack, true);
                    break;
                case State.DIE:
                    // 죽음 처리
                    isDie = true;
                    // 추적 중지
                    agent.isStopped = true;
                    // Die 트리거 발동
                    anim.SetTrigger(hashDie);
                    // 몬스터 콜라이더 비활성화
                    GetComponent<CapsuleCollider>().enabled = false;
                    // 몬스터 펀치 콜라이더 비활성화
                    SphereCollider[] spheres = GetComponentsInChildren<SphereCollider>();
                    foreach (SphereCollider sphere in spheres)
                    {
                        sphere.enabled = false;
                    }

                    // 일정 시간 대기 후 오브젝트 풀링 환원
                    yield return new WaitForSeconds(3.0f);

                    // 몬스터 비활성화
                    this.gameObject.SetActive(false);
                    break;
                case State.PLAYERDIE:
                    StopAllCoroutines();

                    // 추적 정지
                    agent.isStopped = true;
                    anim.SetFloat(hashSpeed, Random.Range(0.8f, 1.3f));
                    anim.SetTrigger(hashPlayerDie);
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            Destroy(collision.gameObject);

            // 피격 리액션 애니 트리거 발동
            anim.SetTrigger(hashHit);

            // 충돌 지점 
            Vector3 pos = collision.GetContact(0).point;
            // 총알 충돌 지점의 법선 벡터
            Quaternion rot = Quaternion.LookRotation(-collision.GetContact(0).normal);

            ShowBloodEffect(pos, rot);

            // 몬스터의 hp 차감
            currHp -= 10;
            if (currHp <= 0)
            {
                GameManager.GetInstance().AddScore(1);
                state = State.DIE;
            }
        }
    }

    void ShowBloodEffect(Vector3 pos, Quaternion rot)
    {
        // 혈흔 효과 생성
        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot, monsterTransform);
        Destroy(blood, 1.0f);
    }

    void OnPlayerDie()
    {
        state = State.PLAYERDIE;
    }

    void OnDrawGizmos()
    {
        if (state == State.TRACE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(monsterTransform.position, traceDist);
        }
        if (state == State.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(monsterTransform.position, attackDist);
        }
    }
}
