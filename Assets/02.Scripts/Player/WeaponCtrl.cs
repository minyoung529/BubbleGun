using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class WeaponCtrl : MonoBehaviour
{
    [Header("Gun")]
    public GameObject bulletPrefab;
    public Transform firePos;
    private GameObject muzzleFlash;

    [SerializeField] private UnityEvent onPlayerShoot;

    [SerializeField] List<GameObject> weapons;

    private Action[] weaponAttacks = new Action[(int)WeaponType.Count];
    [SerializeField] private Sound[] shootSounds;

    private void Start()
    {
        muzzleFlash = firePos.GetChild(0).gameObject;
        muzzleFlash.SetActive(false);

        weaponAttacks[0] = new Action(Shoot);
        weaponAttacks[1] = new Action(Hammer);
    }

    void Update()
    {
        if (GameManager.Instance.GameState != GameState.Game) return;

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    public void Attack()
    {
        weaponAttacks[(int)PlayerController.WeaponType].Invoke();

        Sound sound = shootSounds[(int)PlayerController.WeaponType];
        SoundManager.Instance.PlayOneShot(sound.chanel, sound.clip, 0.5f);
    }

    public void Shoot()
    {
        onPlayerShoot.Invoke();
        StartCoroutine(ShowMuzzleFlash());
        BulletCtrl bullet = PoolManager.Pop(bulletPrefab, firePos.position, transform.rotation).GetComponent<BulletCtrl>();
        bullet.IsPlayerBullet = true;
    }

    IEnumerator ShowMuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        muzzleFlash.SetActive(false);
    }

    public void Hammer()
    {
        onPlayerShoot.Invoke();
    }

    public void ChangeWeapon(WeaponType weapon)
    {
        weapons.ForEach(x => x.gameObject.SetActive(false));
        weapons[(int)weapon].SetActive(true);
    }
}
