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

            ContactPoint cp = collision.GetContact(0);

            Quaternion rotSpark = Quaternion.LookRotation(-cp.normal);
            
            // 스파크 파티클 생성
            GameObject spark = Instantiate(sparkEffect, cp.point, rotSpark);

            Destroy(spark, 0.5f);

            Destroy(collision.gameObject);
        }
    }
}
