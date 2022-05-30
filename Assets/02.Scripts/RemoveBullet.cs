using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    // ����ũ ��ƼŬ ������ ���� �Լ�
    public GameObject sparkEffect;

    private void OnCollisionEnter(Collision collision)
    {
        if( collision.collider.CompareTag("BULLET"))
        {

            ContactPoint cp = collision.GetContact(0);

            Quaternion rotSpark = Quaternion.LookRotation(-cp.normal);
            
            // ����ũ ��ƼŬ ����
            GameObject spark = Instantiate(sparkEffect, cp.point, rotSpark);

            Destroy(spark, 0.5f);

            Destroy(collision.gameObject);
        }
    }
}
