using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    // 스파크 파티클 프리팹 연결 함수
    public GameObject sparkEffect;

    private void OnCollisionEnter(Collision collision)
    {
        if( collision.collider.CompareTag("BULLET"))
        {
            PoolManager.Push(collision.gameObject);
        }
    }
}
