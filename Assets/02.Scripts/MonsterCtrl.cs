using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MonsterCtrl : MonoBehaviour
{
    // ������ ���� ����
    public enum State
    {
        IDLE, 
        TRACE,
        ATTACK,
        DIE,
        PLAYERDIE,
    }

    // ������ ���� ����
    public State state = State.IDLE;
    // ���� ���� �Ÿ�
    public float traceDist = 10.0f;
    // ���� ���� �Ÿ�
    public float attackDist = 2.0f;
    // ������ ��� ����
    public bool isDie = false;

    private Transform monsterTransform;
    private Transform targetTransform;
    private NavMeshAgent agent;
    private Animator anim;

    // Animator �ؽ� �� ����
    private readonly int hashTrace = Animator.StringToHash("IsTrace");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");

    // ���� ȿ�� ������
    private GameObject bloodEffect;

    // ���� ���� �ʱⰪ
    private int initHp = 100;
    private int currHp;

    void Awake()
    {
        monsterTransform = GetComponent<Transform>();

        targetTransform = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();

        agent = GetComponent<NavMeshAgent>();

        // NavMeshAgent �ڵ� ȸ�� ��� ��Ȱ��ȭ
        agent.updateRotation = false;

        anim = GetComponent<Animator>();

        bloodEffect = Resources.Load<GameObject>("BloodSprayEffect");
    }

    void OnEnable()
    {
        state = State.IDLE;

        currHp = initHp;
        isDie = false;          
        
        // ���� �ݶ��̴� Ȱ��ȭ
        GetComponent<CapsuleCollider>().enabled = true;
        // ���� ��ġ �ݶ��̴� Ȱ��ȭ
        SphereCollider[] spheres = GetComponentsInChildren<SphereCollider>();
        foreach (SphereCollider sphere in spheres)
        {
            sphere.enabled = true;
        }

        // ������ ���¸� üũ�ϴ� �ڷ�ƾ �Լ�
        StartCoroutine(CheckMonsterState());

        // ���¿� ���� ������ �ൿ�� �����ϴ� �ڷ�ƾ �Լ�
        StartCoroutine(MonsterAction());
    }

    void Update()
    {
        // ���������� ���� �Ÿ��� ȸ�� ���� �Ǵ�
        if( agent.remainingDistance >= 2.0f)
        {
            // �������� �̵� ȸ��
            Vector3 direction = agent.desiredVelocity;

            Quaternion rot = Quaternion.LookRotation(direction);

            monsterTransform.rotation = Quaternion.Slerp(monsterTransform.rotation, rot, Time.deltaTime * 10.0f);
        }
    }

    IEnumerator CheckMonsterState()
    {
        while(!isDie)
        {
            yield return new WaitForSeconds(0.3f);

            // ���� ���°� DIE �ڷ�ƾ ����
            if (state == State.DIE)
                yield break;

            if (state == State.PLAYERDIE)
                yield break;

            // ���Ϳ� ���ΰ� ĳ���� ������ �Ÿ� ����
            float distance = Vector3.Distance(monsterTransform.position, targetTransform.position);

            if( distance <= attackDist )
            {
                state = State.ATTACK;
            }
            else if( distance <= traceDist )
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
        while(!isDie)
        {
            switch(state)
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
                    // ���� ó��
                    isDie = true;
                    // ���� ����
                    agent.isStopped = true;
                    // Die Ʈ���� �ߵ�
                    anim.SetTrigger(hashDie);
                    // ���� �ݶ��̴� ��Ȱ��ȭ
                    GetComponent<CapsuleCollider>().enabled = false;
                    // ���� ��ġ �ݶ��̴� ��Ȱ��ȭ
                    SphereCollider[] spheres = GetComponentsInChildren<SphereCollider>();
                    foreach(SphereCollider sphere in spheres)
                    {
                        sphere.enabled = false;
                    }

                    // ���� �ð� ��� �� ������Ʈ Ǯ�� ȯ��
                    yield return new WaitForSeconds(3.0f);

                    // ���� ��Ȱ��ȭ
                    this.gameObject.SetActive(false);
                    break;
                case State.PLAYERDIE:
                    StopAllCoroutines();

                    // ���� ����
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
        if( collision.collider.CompareTag("BULLET"))
        {
            Destroy(collision.gameObject);

            // �ǰ� ���׼� �ִ� Ʈ���� �ߵ�
            anim.SetTrigger(hashHit);

            // �浹 ���� 
            Vector3 pos = collision.GetContact(0).point;
            // �Ѿ� �浹 ������ ���� ����
            Quaternion rot = Quaternion.LookRotation(-collision.GetContact(0).normal);

            ShowBloodEffect(pos, rot);

            // ������ hp ����
            currHp -= 10;
            if( currHp <= 0 )
            {
                state = State.DIE;

                GameMgr.GetInstance().DisplayScore(50);
            }    
        }
    }

    void ShowBloodEffect(Vector3 pos, Quaternion rot)
    {
        // ���� ȿ�� ����
        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot, monsterTransform);
        Destroy(blood, 1.0f);
    }

    void OnPlayerDie()
    {
        state = State.PLAYERDIE;
    }

    void OnDrawGizmos()
    {
        if( state == State.TRACE )
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
