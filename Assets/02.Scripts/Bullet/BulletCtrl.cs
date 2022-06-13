using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    // ÃÑ¾Ë ¹ß»ç Èû
    public float force = 1500.0f;
    private Rigidbody bulletRigidbody = null;
    private TrailRenderer trailRenderer;

    private void OnEnable()
    {
        bulletRigidbody ??= GetComponent<Rigidbody>();
        trailRenderer ??= GetComponentInChildren<TrailRenderer>();

        bulletRigidbody.velocity = Vector3.zero;
        trailRenderer?.gameObject.SetActive(true);
        
        StartCoroutine(WaitOneFrame());
        PoolManager.Push(gameObject, 10f);
    }

    private IEnumerator WaitOneFrame()
    {
        yield return null;
        bulletRigidbody.AddForce(transform.forward * force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("PLATFORM"))
        {
            PoolManager.Push(gameObject);
        }
    }

    private void OnDisable()
    {
        trailRenderer?.gameObject.SetActive(false);
    }
}
