using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    // ÃÑ¾Ë ÆÄ±«·Â
    public float damage = 20.0f;

    // ÃÑ¾Ë ¹ß»ç Èû
    public float force = 1500.0f;

    private Rigidbody bulletRigidbody = null;
    private Transform bulletTransform = null;

    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletTransform = GetComponent<Transform>();

        // ÃÑ¾ËÀÇ ÀüÁø¹æÇâÀ¸·Î ÈûÀ» °¡ÇÑ´Ù.
        bulletRigidbody.AddForce(bulletTransform.forward * force);

        Destroy(gameObject, 10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("PLATFORM"))
        {
            Destroy(gameObject);
        }
    }
}
