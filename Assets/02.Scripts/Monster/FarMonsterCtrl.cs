using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarMonsterCtrl : MonsterCtrl
{
    private float timer = 0f;
    private float fireDelay = 3f;
    [SerializeField] private BulletCtrl bullet;
    [SerializeField] private Transform bulletPos;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnAttack()
    {
        base.OnAttack();

        timer += Time.deltaTime;

        if (timer > fireDelay * Time.deltaTime / 0.3f)
        {
            timer = 0f;
            bulletPos.LookAt(targetTransform);
            Instantiate(bullet, bulletPos.position, bulletPos.rotation, null);
        }
    }
}
