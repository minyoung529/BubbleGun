using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossController : MonoBehaviour
{
    private const int maxHp = 100;
    private int BossHp { get; set; } = maxHp;

    private bool isAttack = false;
    public static bool IsDead { get; set; } = false;
    public List<Cucumber> lasers;

    public Transform boss;
    Transform targetTransform;

    private GameObject spotLight;
    public GameObject bulletPrefab;

    private string[] attacks = new string[2];

    int curAttack = 0;

    void Awake()
    {
        Debug.Log(FindObjectsOfType<BossController>().Length);

        EventManager.StartListening("Boss", BossActive);
        spotLight = Resources.Load<GameObject>("EnemySpotLight");

        attacks[0] = "IntervalAttack";
        attacks[1] = "Laser";

        boss.gameObject.SetActive(false);
    }

    private void Start()
    {
        targetTransform = GameManager.Instance.PlayerController.transform;
        GameManager.Instance.UIManager.UpdateBossHp(1, 1);
    }

    private void Update()
    {
        if (!isAttack && !IsDead)
        {
            StartCoroutine(attacks[curAttack++ % attacks.Length]);
        }
    }

    private void BossActive()
    {
        boss ??= transform.parent;
        boss.gameObject.SetActive(true);
        boss.position -= Vector3.up * 12f;
        boss.DOMoveY(0f, 1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("BULLET"))
        {
            BossHp -= 1;
            GameManager.Instance.UIManager.UpdateBossHp(maxHp, BossHp);

            if (BossHp <= 0)
            {
                GameManager.Instance.ClearBoss();
                GameManager.Instance.OnEnemyDie();
                StopAllCoroutines();
                IsDead = true;

                lasers.ForEach(x => x.gameObject.SetActive(false));
                boss.DOKill();
                boss.DORotate(new Vector3(-85f, -30f, 0f), 2f);
            }
        }
    }

    private IEnumerator IntervalAttack()
    {
        isAttack = true;
        yield return new WaitForSeconds(2.5f);

        int count = Random.Range(2, 4);

        for (int i = 0; i < count; i++)
        {
            GameObject light = PoolManager.Pop(spotLight);
            light.transform.position = targetTransform.position + Vector3.up * 5f;
            yield return new WaitForSeconds(0.8f);

            GameObject bullet = PoolManager.Pop(bulletPrefab);
            bullet.transform.position = light.transform.position + Vector3.up * 10f;

            yield return new WaitForSeconds(1f);

            PoolManager.Push(light);
            PoolManager.Push(bullet);
        }
        isAttack = false;
    }

    private IEnumerator Laser()
    {
        isAttack = true;
        lasers.ForEach(x => x.Ready());

        yield return new WaitForSeconds(2.5f);

        Debug.Log("sdf");

        int count = Random.Range(2, 4);
        lasers.ForEach(x => x.gameObject.SetActive(true));
        for (int i = 0; i < count; i++)
        {
            lasers.ForEach(x => x.Ready());
            yield return new WaitForSeconds(0.7f);
            lasers.ForEach(x => x.OnActive());
            boss.DOLookAt(boss.position - boss.forward, 4f);
            yield return new WaitForSeconds(4f);

            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }

        lasers.ForEach(x => x.gameObject.SetActive(false));

        isAttack = false;
    }
}