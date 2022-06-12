using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarMonsterCtrl : MonsterCtrl
{
    [SerializeField] private BulletCtrl bullet;
    [SerializeField] private Transform bulletPos;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnAttack()
    {
        base.OnAttack();
    }

    // 애니메이션에서 Event로 재생
    public void Attack()
    {
        bulletPos.LookAt(targetTransform);
        Instantiate(bullet, bulletPos.position, bulletPos.rotation, null);
    }
}
