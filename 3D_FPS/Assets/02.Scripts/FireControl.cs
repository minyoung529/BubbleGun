using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireControl : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePos;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    private void Fire()
    {
        Instantiate(bulletPrefab, firePos.position, firePos.rotation);
    }
}
