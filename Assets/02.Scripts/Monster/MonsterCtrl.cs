using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using DG.Tweening;

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
    protected State state = State.IDLE;
    public State MonsterState { get => state; set => state = value; }

    // ���� ���� �Ÿ�
    [SerializeField]
    protected float traceDist = 10.0f;
    // ���� ���� �Ÿ�

    [SerializeField]
    protected float attackDist = 2.0f;
    // ������ ��� ����
    protected bool isDie = false;

    protected Transform targetTransform;
    protected NavMeshAgent agent;

    // ���� ���� �ʱⰪ
    [SerializeField]
    protected int initHp = 100;
    protected int currHp;

    [SerializeField] protected GameObject gumItem;

    public bool IsMove { get; set; } = true;

    private Sequence stunSequence;

    [SerializeField] protected UnityEvent onGotHit;
    [SerializeField] protected UnityEvent onPlayerDie;
    [SerializeField] protected UnityEvent onAttack;
    [SerializeField] protected UnityEvent onDie;
    [SerializeField] protected UnityEvent onTrace;
    [SerializeField] protected UnityEvent onStun;
    [SerializeField] protected UnityEvent onIdle;

    protected virtual void Awake()
    {
        targetTransform = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    protected virtual void OnEnable()
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

    protected virtual void Update()
    {
        // ���������� ���� �Ÿ��� ȸ�� ���� �Ǵ�
        if (!IsMove) return;

        if (state == State.ATTACK || state == State.TRACE)
        {
            //Quaternion rot = Quaternion.LookRotation(targetTransform.position);
            //transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 10.0f);

            transform.DOLookAt(targetTransform.position, 0.1f);
        }
    }

    protected IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.3f);

            // ���� ���°� DIE �ڷ�ƾ ����
            if (state == State.DIE)
                yield break;

            if (state == State.PLAYERDIE)
                yield break;

            // ���Ϳ� ���ΰ� ĳ���� ������ �Ÿ� ����
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
        if (collision.collider.CompareTag("BULLET") || collision.collider.CompareTag("HAMMER"))
        {
            onGotHit.Invoke();

            float stunTime = collision.collider.CompareTag("BULLET") ? 0.5f : 1f;

            if (stunSequence != null)
                stunSequence.Restart();
            else
            {
                stunSequence = DOTween.Sequence().SetAutoKill(false);
                stunSequence.AppendCallback(() => IsMove = false);
                stunSequence.InsertCallback(stunTime, () => IsMove = true);
            }

            currHp -= 10;
            if (currHp <= 0)
            {
                state = State.DIE;
            }

            if (collision.collider.CompareTag("BULLET"))
            {
                PoolManager.Push(collision.gameObject);
            }
        }
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

        onPlayerDie.Invoke();
    }

    protected virtual void OnAttack()
    {
        onAttack.Invoke();

        Quaternion rot = Quaternion.LookRotation(targetTransform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 10.0f);
    }

    protected virtual void OnIdle()
    {
        agent.isStopped = false;
        agent.isStopped = true;

        onIdle.Invoke();
    }

    protected virtual void OnTrace()
    {
        agent.SetDestination(targetTransform.position);
        agent.isStopped = false;

        onTrace.Invoke();
    }

    protected virtual IEnumerator OnDie()
    {
        isDie = true;
        agent.isStopped = true;
        onDie.Invoke();
        GetComponent<CapsuleCollider>().enabled = false;

        SphereCollider[] spheres = GetComponentsInChildren<SphereCollider>();
        foreach (SphereCollider sphere in spheres)
        {
            sphere.enabled = false;
        }

        GameManager.Instance.OnEnemyDie();

        yield return new WaitForSeconds(3.0f);

        gameObject.SetActive(false);
    }

    public virtual void Stun()
    {
        IsMove = false;
        agent.isStopped = true;
        onStun.Invoke();
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