using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLC : MonoBehaviour
{
    Rigidbody rigid;
    public bool isTransform;
    public bool isRigidbody;
    public float speed;

    float rotY = 0f;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (isTransform)
        {
            transform.Translate(new Vector3(0f, 0f, v) * Time.deltaTime * speed);
            //transform.Rotate(new Vector3(0f, h, 0f));
            rotY += h;
            //transform.rotation = Quaternion.Euler(0f, rotY, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, rotY, 0f), Time.deltaTime * 3f);
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, rotY, 0f), Time.deltaTime * 3f);
        }

        if (isRigidbody)
        {
            //rigid.velocity = new Vector3(h, rigid.velocity.y, v);
            rigid.MovePosition(rigid.position + new Vector3(h, 0f, v) * Time.deltaTime);
        }
    }
}
