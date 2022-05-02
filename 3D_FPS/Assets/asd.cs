using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asd : MonoBehaviour
{
    void Update()
    {
        transform.Translate(transform.up * Time.deltaTime * 10f, Space.World);
    }
}
