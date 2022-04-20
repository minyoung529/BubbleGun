using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    public GameObject sparkleEffectPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            ContactPoint cp = collision.GetContact(0);

            Quaternion rotation = Quaternion.LookRotation(-cp.normal);
                
            GameObject effect = Instantiate(sparkleEffectPrefab, collision.transform.position, rotation);

            Destroy(effect, 0.5f);
            Destroy(collision.gameObject);
        }
    }
}
