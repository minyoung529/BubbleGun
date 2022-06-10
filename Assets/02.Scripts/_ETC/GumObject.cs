using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GumObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("PLAYER"))
        {
            GameManager.Instance.AddScore(1);
            Destroy(gameObject);
        }
    }
}
