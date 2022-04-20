using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float damage;
    public float force = 1500f;

    private Rigidbody rigid;
    private Transform bulletTransform;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        bulletTransform = GetComponent<Transform>();

        rigid.AddForce(bulletTransform.forward * force);
    }
}
