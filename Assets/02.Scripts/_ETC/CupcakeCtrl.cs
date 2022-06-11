using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupcakeCtrl : MonoBehaviour
{
    // ���� ȿ�� ��ƼŬ ����
    public GameObject expEffect;

    // �������� ������ �ؽ�ó �迭
    public GameObject[] objects;

    // �����ϴ� ��
    public float expForce = 1500.0f;

    // ���� �ݰ�
    public float radius = 10.0f;

    // �Ѿ� ���� Ƚ�� ����
    private int hitCount = 0;

    private Transform barrelTransform = null;
    private Rigidbody barrelRigidbody = null;

    private Collider[] colls = new Collider[10];

    void Start()
    {
        barrelTransform = GetComponent<Transform>();
        barrelRigidbody = GetComponent<Rigidbody>();

        int index = Random.Range(0, objects.Length);

        GameObject obj = Instantiate(objects[index], transform);
        obj.transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
        obj.transform.localScale = Vector3.one * Random.Range(0.8f, 1.3f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            // �Ѿ� ���� Ƚ���� ������Ű�� 3ȸ�� ���� ó��
            if (++hitCount == 3)
            {
                ExpBarrel();
            }
        }
    }

    void ExpBarrel()
    {
        GameObject exp = Instantiate(expEffect, barrelTransform.position, Quaternion.identity);
        Destroy(exp, 5.0f);

        barrelRigidbody.mass = 1.0f;
        barrelRigidbody.AddForce(Vector3.up * expForce);

        // ���� ���߷� ����
        IndirectDamage(barrelTransform.position);

        Destroy(gameObject, 3.0f);
    }

    void IndirectDamage(Vector3 pos)
    {
        // �ֺ��� �ִ� �巳���� ����
        //Collider[] colls = Physics.OverlapSphere(pos, radius, 1 << 8);
        Physics.OverlapSphereNonAlloc(pos, radius, colls, 1 << 8);

        foreach (var coll in colls)
        {
            if (coll == null)
                continue;

            Rigidbody rigid = coll.GetComponent<Rigidbody>();

            // ���� ������
            rigid.mass = 1.0f;
            rigid.constraints = RigidbodyConstraints.None;

            // ���߷� ����
            rigid.AddExplosionForce(1500.0f, pos, radius, 1200.0f);
        }
    }
}
