using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingObject : MonoBehaviour
{
    private Color color;

    private void Start()
    {
        color = GetComponent<Renderer>().material.GetColor("_BaseColor");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Paintable paintableObject = collision.transform.GetComponentInChildren<Paintable>();

        if (!paintableObject) return;

        Vector3 point = collision.contacts[0].point;

        Debug.Log("Paint");
        GameManager.GetInstance().PaintManager.Paint(paintableObject, point, 0.5f, 1, 1, color);
    }
}
