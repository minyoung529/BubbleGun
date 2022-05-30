using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    // ���� ȿ�� ��ƼŬ ����
    public GameObject expEffect;

    // �������� ������ �ؽ�ó �迭
    public Texture[] textures;

    private new MeshRenderer renderer;

    // �����ϴ� ��
    public float expForce = 1500.0f;

    // ���� �ݰ�
    public float radius = 10.0f;

    // �Ѿ� ���� Ƚ�� ����
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
            // �Ѿ� ���� Ƚ���� ������Ű�� 3ȸ�� ���� ó��
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

        // ���� ���߷� ����
        IndirectDamage(barrelTransform.position);

        Destroy(this.gameObject, 3.0f);
    }

    Collider[] colls = new Collider[10];
    void IndirectDamage(Vector3 pos)
    {
        // �ֺ��� �ִ� �巳���� ����
        //Collider[] colls = Physics.OverlapSphere(pos, radius, 1 << 8);
        Physics.OverlapSphereNonAlloc(pos, radius, colls, 1 << 8);

        foreach( var coll in colls )
        {
            if (coll == null)
                continue;

            Rigidbody rb = coll.GetComponent<Rigidbody>();

            // ���� ������
            rb.mass = 1.0f;
            rb.constraints = RigidbodyConstraints.None;

            // ���߷� ����
            rb.AddExplosionForce(1500.0f, pos, radius, 1200.0f);
        }
    }
}
