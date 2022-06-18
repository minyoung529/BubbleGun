using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float speed = 0.1f;

    private void Update()
    {
        transform.Translate(-transform.forward * Time.deltaTime * speed);
    }
}
