using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelControl : MonoBehaviour
{
    public GameObject expEffect;
    public float expForce = 1500f;
    private int hitCount = 0;
    private Transform barrelTransform;
    private Rigidbody rigid;

    void Start()
    {
        barrelTransform = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("BULLET"))
        {
            if(++hitCount >= 3)
                ExpBarrel();
        }
    }

    private void ExpBarrel()
    {
        GameObject exp = Instantiate(expEffect, barrelTransform.position, Quaternion.identity);
        Destroy(exp, 5f);

        rigid.mass = 1f;
        rigid.AddForce(Vector3.up * expForce);

        Destroy(gameObject, 3f);
    }
}
