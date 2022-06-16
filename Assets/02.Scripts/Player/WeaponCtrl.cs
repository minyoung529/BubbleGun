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
            weaponAttacks[(int)PlayerController.WeaponType].Invoke();
        }
    }

    public void Shoot()
    {
        onPlayerShoot.Invoke();
        StartCoroutine(ShowMuzzleFlash());
        PoolManager.Pop(bulletPrefab, firePos.position, firePos.rotation);
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
