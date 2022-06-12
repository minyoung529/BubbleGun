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
    [SerializeField] private UnityEvent onChangeWeapon;

    [SerializeField] List<GameObject> weapons;

    Action[] weaponAttacks = new Action[(int)WeaponType.Count]; 

    private void Start()
    {
        muzzleFlash = firePos.GetChild(0).gameObject;
        muzzleFlash.SetActive(false);

        weaponAttacks[0] = new Action(Shoot);
        weaponAttacks[1] = new Action(Hammer);
    }

    void Update()
    {
        if (Mathf.Abs(Input.GetAxisRaw("Mouse ScrollWheel")) >= 0.1f)
        {
            int nextType = (int)PlayerController.WeaponType + 1;
            PlayerController.WeaponType = (WeaponType)(nextType % (int)WeaponType.Count);

            onChangeWeapon.Invoke();
            weapons.ForEach(x => x.SetActive(false));
            weapons[(int)PlayerController.WeaponType].SetActive(true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            weaponAttacks[(int)PlayerController.WeaponType].Invoke();
        }
    }

    public void Shoot()
    {
        onPlayerShoot.Invoke();
        StartCoroutine(ShowMuzzleFlash());

        Instantiate(bulletPrefab, firePos.position, firePos.rotation);
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
}