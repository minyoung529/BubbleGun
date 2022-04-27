using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelControl : MonoBehaviour
{
    public GameObject expEffect;

    //Random Texture[]
    public Texture[] textures = new Texture[10];

    private new MeshRenderer renderer;

    public float expForce = 1500f;
    public float expRadius = 10;

    private int hitCount = 0;
    private Transform barrelTransform;
    private Rigidbody rigid;

    Collider[] cols;

    void Start()
    {
        barrelTransform = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody>();
        renderer = GetComponentInChildren<MeshRenderer>();

        renderer.material.mainTexture = textures[Random.Range(0, textures.Length)];
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            if (++hitCount >= 3)
                ExpBarrel();
        }
    }

    private void ExpBarrel()
    {
        GameObject exp = Instantiate(expEffect, barrelTransform.position, Quaternion.identity);
        Destroy(exp, 5f);

        IndirectDamage(barrelTransform.position);

        //rigid.mass = 1f;
        //rigid.AddForce(Vector3.up * expForce);

        Destroy(gameObject, 3f);
    }

    private void IndirectDamage(Vector3 position)
    {
        Physics.OverlapSphereNonAlloc(position, expRadius, cols, LayerMask.GetMask("BARREL"));

        if (cols == null) return;

        foreach (Collider col in cols)
        {
            if (col == null) continue;
            Rigidbody rb = col.GetComponent<Rigidbody>();
            rb.mass = 1f;
            rb.constraints = RigidbodyConstraints.None;
            rb.AddExplosionForce(1500f, position, expRadius, 1200f);
        }
    }
}
