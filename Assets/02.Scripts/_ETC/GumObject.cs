using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GumObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("PLAYER"))
        {
            GameManager.GetInstance().AddScore(1);
            Destroy(collision.gameObject);
        }
    }
}
