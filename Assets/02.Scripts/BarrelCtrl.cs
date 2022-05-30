using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    // 폭발 효과 파티클 변수
    public GameObject expEffect;

    // 무작위로 적용할 텍스처 배열
    public Texture[] textures;

    private new MeshRenderer renderer;

    // 폭발하는 힘
    public float expForce = 1500.0f;

    // 폭발 반경
    public float radius = 10.0f;

    // 총알 맞은 횟수 변수
    private int hitCount = 0;

    private Transform barrelTransform = null;
    private Rigidbody barrelRigidbody = null;

    void Start()
    {
        barrelTransform = GetComponent<Transform>();
        barrelRigidbody = GetComponent<Rigidbody>();

        renderer = GetComponentInChildren<MeshRenderer>();

        int index = Random.Range(0, textures.Length);

        renderer.material.mainTexture = textures[index];
    }

    private void OnCollisionEnter(Collision collision)
    {
        if( collision.collider.CompareTag("BULLET"))
        {
            // 총알 맞은 횟수를 증가시키고 3회시 폭발 처리
            if (++hitCount == 3)
                ExpBarrel();
        }
    }

    void ExpBarrel()
    {
        GameObject exp = Instantiate(expEffect, barrelTransform.position, Quaternion.identity);

        Destroy(exp, 5.0f);

        //barrelRigidbody.mass = 1.0f;
        //barrelRigidbody.AddForce(Vector3.up * expForce);

        // 간접 폭발력 전달
        IndirectDamage(barrelTransform.position);

        Destroy(this.gameObject, 3.0f);
    }

    Collider[] colls = new Collider[10];
    void IndirectDamage(Vector3 pos)
    {
        // 주변에 있는 드럼통을 추출
        //Collider[] colls = Physics.OverlapSphere(pos, radius, 1 << 8);
        Physics.OverlapSphereNonAlloc(pos, radius, colls, 1 << 8);

        foreach( var coll in colls )
        {
            if (coll == null)
                continue;

            Rigidbody rb = coll.GetComponent<Rigidbody>();

            // 질량 가볍게
            rb.mass = 1.0f;
            rb.constraints = RigidbodyConstraints.None;

            // 폭발력 전달
            rb.AddExplosionForce(1500.0f, pos, radius, 1200.0f);
        }
    }
}
