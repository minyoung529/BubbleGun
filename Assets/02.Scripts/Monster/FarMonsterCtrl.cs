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

    // �ִϸ��̼ǿ��� Event�� ���
    public void Attack()
    {
        bulletPos.LookAt(targetTransform);
        Instantiate(bullet, bulletPos.position, bulletPos.rotation, null);
    }
}
