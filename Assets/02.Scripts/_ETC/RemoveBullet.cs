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
            PoolManager.Push(collision.gameObject);
        }
    }
}
