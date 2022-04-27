using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BOXBOX : MonoBehaviour
{
    public Transform targetPos;
    Rigidbody rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 vel = Vector3.zero;
        //transform.position = Vector3.MoveTowards(transform.position, targetPos.position, Time.deltaTime);
        //transform.position = Vector3.Slerp(transform.position, targetPos.position, Time.deltaTime);
        //transform.position = Vector3.Lerp(transform.position, targetPos.position, Time.deltaTime);
        //transform.position = Vector3.SmoothDamp(transform.position, targetPos.position, ref vel, Time.deltaTime* 10f);

        //transform.RotateAround(targetPos.position, targetPos.up, Time.deltaTime * 20);
        //transform.RotateAround(targetPos.position, targetPos.up, Time.deltaTime * 20);

        //rigid.MovePosition(targetPos.position);
    }
}
