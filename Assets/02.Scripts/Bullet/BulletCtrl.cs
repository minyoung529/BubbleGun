using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    // �Ѿ� �ı���
    public float damage = 20.0f;

    // �Ѿ� �߻� ��
    public float force = 1500.0f;

    private Rigidbody bulletRigidbody = null;
    private Transform bulletTransform = null;
    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletTransform = GetComponent<Transform>();

        // �Ѿ��� ������������ ���� ���Ѵ�.
        bulletRigidbody.AddForce(bulletTransform.forward * force);
    }
}
