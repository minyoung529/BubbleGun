using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    // ?Ѿ? ?߻? ??
    public float force = 1500.0f;
    private Rigidbody bulletRigidbody = null;
    private TrailRenderer trailRenderer;

    public bool IsPlayerBullet = false;

    private void OnEnable()
    {
        bulletRigidbody ??= GetComponent<Rigidbody>();
        trailRenderer ??= GetComponentInChildren<TrailRenderer>();

        StartCoroutine(WaitOneFrame());

        PoolManager.Push(gameObject, 10f);
    }

    private IEnumerator WaitOneFrame()
    {
        yield return null;

        trailRenderer?.Clear();
        bulletRigidbody.WakeUp();

        if (IsPlayerBullet)
            bulletRigidbody.AddForce(GameManager.Instance.PlayerController.transform.forward * force);

        else
            bulletRigidbody.AddForce(transform.forward * force);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLATFORM"))
        {
            PoolManager.Push(gameObject);
        }
    }

    private void OnDisable()
    {
        IsPlayerBullet = false;
        bulletRigidbody.velocity = Vector3.zero;
        bulletRigidbody.angularVelocity = Vector3.zero;
        bulletRigidbody.Sleep();
        trailRenderer?.Clear();
        trailRenderer?.gameObject.SetActive(false);
    }
}
