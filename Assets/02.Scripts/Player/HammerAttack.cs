using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HammerAttack : MonoBehaviour
{
    [SerializeField] private Transform trail;
    private float angle = 0f;
    private bool isAttack = false;

    [SerializeField] int count;
    [SerializeField] float distance = 7f;
    [SerializeField] List<ParticleSystem> particles;

    private List<MonsterCtrl> monsterCtrls;

    private void Start()
    {
        trail.localPosition = Vector3.forward * distance;
    }

    private void Update()
    {
        if (isAttack)
        {
            angle += Time.deltaTime * 360f;
            trail.RotateAround(transform.parent.position, Vector3.up, angle * Time.deltaTime * 2f);

            if (angle >= 360f * count)
            {
                DamageMonster();
                angle = 0f;

                monsterCtrls.Clear();
                isAttack = false;
                trail.gameObject.SetActive(false);
                GameManager.Instance.PlayerController.SetWeaponType(WeaponType.Gun);
            }
        }
    }

    public void Attack()
    {
        if (PlayerController.WeaponType != WeaponType.Hammer) return;

        particles.ForEach(x => x.Play());
        DamageMonster();
        trail.gameObject.SetActive(true);
        isAttack = true;
    }

    private void DamageMonster()
    {
        Predicate<MonsterCtrl> match = x => Vector3.Distance(x.transform.position, transform.parent.position) < distance;
        monsterCtrls = GameManager.Instance.CurrentMonster.FindAll(match);
        monsterCtrls.ForEach(x => x.MonsterState = MonsterCtrl.State.DIE);
    }
}