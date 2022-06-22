using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossController : MonoBehaviour
{
    private const int maxHp = 200;
    private int BossHp { get; set; } = maxHp;

    private bool isAttack;
    public List<Transform> lasers;

    public Transform bossTransform;
    Transform targetTransform;

    private GameObject spotLight;
    public GameObject bulletPrefab;

    void Start()
    {
        EventManager.StartListening("Boss", BossActive);
        gameObject.SetActive(false);
        targetTransform = GameManager.Instance.PlayerController.transform;
        spotLight = Resources.Load<GameObject>("SpotLight");
    }

    private void Update()
    {
        if (!isAttack)
        {
            StartCoroutine(IntervalAttack());
        }
    }

    private void BossActive()
    {
        gameObject.SetActive(true);
        bossTransform.position -= Vector3.up * 12f;
        bossTransform.DOMoveY(0f, 1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("BULLET"))
        {
            Debug.Log("sfd");
            BossHp -= 1;
            GameManager.Instance.UIManager.UpdateBossHp(maxHp, BossHp);

            if (BossHp <= 0)
            {
                EventManager.TriggerEvent("Win");
            }
        }
    }

    private IEnumerator IntervalAttack()
    {
        isAttack = true;
        int count = Random.Range(2, 4);

        for (int i = 0; i < count; i++)
        {
            GameObject light = PoolManager.Pop(spotLight);
            light.transform.position = targetTransform.position + Vector3.up * 5f;
            yield return new WaitForSeconds(1.5f);

            GameObject bullet = PoolManager.Pop(bulletPrefab);
            bullet.transform.position = light.transform.position;

            yield return new WaitForSeconds(1f);

            PoolManager.Push(light);
            PoolManager.Push(bullet);
        }
        isAttack = false;
    }

    private IEnumerator Laser()
    {
        isAttack = true;
        int count = Random.Range(2, 4);
        lasers.ForEach(x => x.gameObject.SetActive(true));
        for (int i = 0; i < count; i++)
        {
            Vector3 randomPos = bossTransform.position;
            randomPos.x += Random.Range(-1f, 1f);
            randomPos.z += Random.Range(-1f, 1f);

            bossTransform.DOLookAt(randomPos, 2.5f);
            yield return new WaitForSeconds(2.5f);

            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }

        lasers.ForEach(x => x.gameObject.SetActive(false));

        isAttack = false;
    }
}
