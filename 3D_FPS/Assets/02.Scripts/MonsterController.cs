using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    //Monster Status
    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE,
        PLAYERDIE
    }

    public State state = State.IDLE;

    public float traceDistance = 10f;
    public float attackDistance = 2f;

    public bool isDie = false;

    private Transform monsterTransform;
    private Transform targetTransform;
    private NavMeshAgent agent;
    private Animator animator;

    private WaitForSeconds delay003 = new WaitForSeconds(0.3f);

    private readonly int traceHash = Animator.StringToHash("IsTrace");
    private readonly int attackHash = Animator.StringToHash("IsAttack");
    private readonly int hitHash = Animator.StringToHash("Hit");
    private readonly int playerDieHash = Animator.StringToHash("PlayerDie");
    private readonly int speedHash = Animator.StringToHash("Speed");
    private readonly int dieHash = Animator.StringToHash("Die");

    private GameObject bloodPrefab;

    private readonly float initHp = 3;
    public float hp = 0;

    void Awake()
    {
        hp = initHp;
        monsterTransform = transform;
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        bloodPrefab = Resources.Load<GameObject>("BloodSprayEffect");
    }

    private IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (state)
            {
                case State.IDLE:
                    agent.isStopped = true;
                    animator.SetBool(traceHash, false);
                    break;

                case State.TRACE:
                    agent.isStopped = false;
                    agent.SetDestination(targetTransform.position);

                    animator.SetBool(traceHash, true);
                    animator.SetBool(attackHash, false);
                    break;

                case State.ATTACK:
                    animator.SetBool(attackHash, true);
                    break;

                case State.DIE:
                    Debug.Log("sfd");
                    isDie = true;
                    agent.isStopped = true;
                    animator.SetTrigger(dieHash);
                    GetComponent<CapsuleCollider>().enabled = false;
                    Collider[] cs = GetComponentsInChildren<SphereCollider>();
                    foreach (var v in cs)
                        v.enabled = false;

                    yield return new WaitForSeconds(3f);
                    gameObject.SetActive(false);

                    break;

                case State.PLAYERDIE:
                    StopAllCoroutines();
                    //추적 정지
                    agent.isStopped = true;
                    animator.SetBool(traceHash, false);
                    animator.SetBool(attackHash, false);

                    animator.SetFloat(speedHash, Random.Range(0.8f, 1.4f));
                    animator.SetTrigger(playerDieHash);
                    break;
            }

            yield return delay003;
        }
    }

    private IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            yield return delay003;
            if (state == State.DIE || state == State.PLAYERDIE) yield break;

            float distance = Vector3.Distance(targetTransform.position, monsterTransform.position);

            if (distance <= attackDistance)
            {
                state = State.ATTACK;
            }
            else if (distance <= traceDistance)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (state == State.TRACE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(monsterTransform.position, traceDistance);
        }
        else if (state == State.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(monsterTransform.position, attackDistance);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("BULLET") && hp > 0f)
        {
            Destroy(collision.gameObject);
            animator.SetTrigger(hitHash);
            hp -= 1.0f;

            if (hp <= 0f)
            {
                Debug.Log("DIE");
                state = State.DIE;
            }

            Vector3 hitPoint = collision.GetContact(0).point;
            Quaternion rot = Quaternion.LookRotation(-collision.GetContact(0).normal);

            ShowBloodEffect(hitPoint, rot);
        }
    }

    void ShowBloodEffect(Vector3 pos, Quaternion rot)
    {
        GameObject effect = Instantiate(bloodPrefab, pos, rot, monsterTransform);
        Destroy(effect, 1.5f);
    }

    public void OnPlayerDie()
    {
        state = State.PLAYERDIE;
    }

    private void OnEnable()
    {
        isDie = false;
        hp = initHp;
        state = State.IDLE;

        GetComponent<CapsuleCollider>().enabled = true;
        Collider[] cs = GetComponentsInChildren<SphereCollider>();
        foreach (var v in cs)
            v.enabled = true;

        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
    }
}
